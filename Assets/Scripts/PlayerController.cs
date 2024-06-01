using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float gravity = 98.1f;
    [SerializeField] private float jumpSpeed = 100f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private new Transform camera;
    [SerializeField] private Transform hand;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    private static PlayerController instance;

    private CharacterController characterController;
    private float verticalSpeed = 0f;

    void Start()
    {
        // Time.timeScale = .2f;
        if (instance != null)
        {
            throw new System.Exception("Player already exists");
        }
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        InputManager.actions.Player.Inspect.performed += Inspect;
        InputManager.actions.Player.Drop.performed += Drop;
    }

    private void Update()
    {
        bool grounded = Grounded();
        Vector2 input = InputManager.actions.Player.Move.ReadValue<Vector2>();
        if (input.sqrMagnitude > 1f)
            input.Normalize();

        Vector3 input3d = input.x * transform.right + input.y * transform.forward;
        characterController.Move(input3d * speed * Time.deltaTime);

        verticalSpeed -= gravity * Time.deltaTime;
        if (InputManager.actions.Player.Jump.IsPressed() && grounded)
        {
            verticalSpeed = jumpSpeed;
        }
        characterController.Move(Vector3.up * verticalSpeed * Time.deltaTime);
        if (grounded && verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }
    }

    private void Inspect(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(camera.position, camera.forward), out hit, Mathf.Infinity, groundMask))  // FIXME (Timon): dedicated mask for interactables
        {
            Interactable interactable;
            if (hit.collider.TryGetComponent(out interactable) || hit.collider.transform.parent != null && hit.collider.transform.parent.TryGetComponent(out interactable))
            {
                interactable.Interact();
            }
        }
    }

    bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void Drop(InputAction.CallbackContext context)
    {
        foreach (Transform child in hand)
        {
            Rigidbody rigidbody;
            if (child.TryGetComponent(out rigidbody))
            {
                rigidbody.isKinematic = false;
            }

            child.parent = null;

            HoldableItem holdableItem;
            if (child.TryGetComponent(out holdableItem))
            {
                holdableItem.OnDrop();
            }
        }
    }

    public static void PickUp(Transform item)
    {
        item.parent = instance.hand;
        item.localPosition = Vector3.zero;
        Rigidbody rigidbody;
        if (item.TryGetComponent(out rigidbody))
        {
            rigidbody.isKinematic = true;
        }
    }
}

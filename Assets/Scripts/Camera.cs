using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Light spotLight;

    private void Update()
    {
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        if (angle < spotLight.spotAngle / 2)
        {
            Debug.Log("angle");
        }
    }
}

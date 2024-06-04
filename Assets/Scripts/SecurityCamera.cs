using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Light spotLight;

    private void Update()
    {
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        if (angle < spotLight.spotAngle / 2)
        {
            Debug.Log("angle");
        }
    }
}

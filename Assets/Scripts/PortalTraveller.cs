using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public virtual void Travel(Vector3 newLoc, Quaternion newRot)
    {
        transform.position = newLoc;
        transform.rotation = newRot;
    }
}
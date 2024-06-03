using UnityEngine;

public class InfraRedCamera : HoldableItem
{
    public override void OnInteract()
    {
        base.OnInteract();
        GameObject.Find("Directional Light").GetComponent<Light>().intensity = .5f;
        foreach (Camera camera in Resources.FindObjectsOfTypeAll<Camera>())
        {
            camera.spotLight.intensity = 10f;
        }
    }

    public override void OnDrop()
    {
        GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1f;
        foreach (var camera in Resources.FindObjectsOfTypeAll<Camera>())
        {
            camera.spotLight.intensity = 0f;
        }
    }
}

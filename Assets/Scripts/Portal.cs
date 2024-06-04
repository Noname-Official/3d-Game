using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal linkedPortal;
    public MeshRenderer screen;
    private Camera playerCam;
    private Camera portalCam;
    private RenderTexture texture;

    private List<PortalTraveller> travellers = new List<PortalTraveller>();
    private List<bool> travellerSides = new List<bool>();

    private void Awake()
    {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
        Camera.onPreRender += PreRender;
        Camera.onPostRender += PostRender;
    }

    private void CreateViewTexture()
    {
        if (texture == null || texture.width != Screen.width || texture.height != Screen.height)
        {
            if (texture != null)
            {
                texture.Release();
            }
            texture = new RenderTexture(Screen.width, Screen.height, 0);
            portalCam.targetTexture = texture;
            linkedPortal.screen.material.mainTexture = texture;
        }
    }

    public void Update()
    {
        screen.enabled = false;
        CreateViewTexture();

        Matrix4x4 matrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(matrix.GetColumn(3), matrix.rotation);

        portalCam.Render();

        screen.enabled = true;
    }

    private void LateUpdate()
    {
        var teleportedTravellerIndices = new List<int>();
        foreach (var item in travellers.Zip(travellerSides, (traveller, prevSide) => new { traveller = traveller, prevSide = prevSide }))
        {
            var traveller = item.traveller;
            var prevSide = item.prevSide;
            var curSide = Side(traveller);
            if (curSide != prevSide)
            {
                Debug.Log("Teleport");
                Debug.Log(gameObject);
                Debug.Log(traveller);
                Matrix4x4 matrix = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * traveller.transform.localToWorldMatrix;
                Debug.Log(matrix);
                Debug.Log(matrix.GetColumn(3));
                Debug.Log(matrix.rotation);
                // traveller.transform.SetPositionAndRotation(matrix.GetColumn(3), matrix.rotation);
                traveller.Travel(matrix.GetColumn(3), matrix.rotation);
                teleportedTravellerIndices.Add(travellers.IndexOf(traveller));
            }
        }

        teleportedTravellerIndices.Reverse();
        foreach (var index in teleportedTravellerIndices)
        {
            travellers.RemoveAt(index);
            travellerSides.RemoveAt(index);
        }
    }

    private void PreRender(Camera cam)
    {
        if (cam == portalCam)
        {
            screen.enabled = false;
        }
    }

    private void PostRender(Camera cam)
    {
        if (cam == portalCam)
        {
            screen.enabled = true;
        }
    }

    private bool Side(PortalTraveller traveller)
    {
        return Vector3.Dot(transform.forward, traveller.transform.position - transform.position) < 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PortalTraveller>(out var portalTraveller) && !travellers.Contains(portalTraveller))
        {
            travellers.Add(portalTraveller);
            travellerSides.Add(Side(portalTraveller));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PortalTraveller>(out var portalTraveller) && travellers.Contains(portalTraveller))
        {
            var index = travellers.IndexOf(portalTraveller);
            travellers.RemoveAt(index);
            travellerSides.RemoveAt(index);
        }
    }
}

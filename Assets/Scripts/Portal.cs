using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal linkedPortal;
    public MeshRenderer screen;
    private Camera playerCam;
    private Camera portalCam;
    private RenderTexture texture;

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
}

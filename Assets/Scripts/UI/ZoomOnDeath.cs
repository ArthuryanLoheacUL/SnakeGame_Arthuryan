using UnityEngine;

public class ZoomOnDeath : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Camera camera;

    public void SetupOriginalPosition()
    {
        camera = Camera.main;
        originalPosition = camera.transform.position;
    }

    public void ZoomAtPos(Vector3 _position)
    {
        _position.z = originalPosition.z;
        targetPosition = _position;
    }

    public void ResetZoom()
    {
        targetPosition = originalPosition;
    }

    void Update()
    {
        if (camera == null) return;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, Time.deltaTime * 5f);
    }
}

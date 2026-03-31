using UnityEngine;

public class ShakeCameraManager : MonoBehaviour
{
    public static ShakeCameraManager instance;
    private Camera mainCamera;
    private Vector3 originalCameraPosition;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void ShakeCamera(float _duration, float _magnitude, Vector2 _direction)
    {
        StartCoroutine(Shake(_duration, _magnitude, _direction));
    }

    private System.Collections.IEnumerator Shake(float _duration, float _magnitude, Vector2 _direction)
    {
        originalCameraPosition = mainCamera.transform.localPosition;
        float _elapsed = 0.0f;
        while (_elapsed < _duration)
        {
            float _x = Random.Range(-1f, 1f) * _magnitude * _direction.x;
            float _y = Random.Range(-1f, 1f) * _magnitude * _direction.y;
            mainCamera.transform.localPosition = new Vector3(originalCameraPosition.x + _x, originalCameraPosition.y + _y, originalCameraPosition.z);
            _elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.localPosition = originalCameraPosition;
    }
}

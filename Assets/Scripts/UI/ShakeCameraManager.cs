using UnityEngine;

public class ShakeCameraManager : MonoBehaviour
{
    public static ShakeCameraManager instance;

    private Camera mainCamera;
    private Vector3 originalCameraPosition;

    // Ensure that there is only one instance of ShakeCameraManager and set it to the static instance variable
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

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Shake the camera for a specified duration, magnitude, and direction
    public void ShakeCamera(float _duration, float _magnitude, Vector2 _direction)
    {
        StartCoroutine(Shake(_duration, _magnitude, _direction));
    }

    // Coroutine that handles the actual shaking of the camera
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

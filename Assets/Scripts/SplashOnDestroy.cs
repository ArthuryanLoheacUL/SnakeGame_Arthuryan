using UnityEngine;

public class SplashOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject splashPrefab;

    public void Splash()
    {
        if (splashPrefab != null)
        {
            Vector3 _pos = transform.position;
            _pos.y -= 1f;

            Instantiate(splashPrefab, _pos, Quaternion.identity);
        }
    }
}

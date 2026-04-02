using UnityEngine;

public class DestroyAfterX : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}

using UnityEngine;

public class DestroyAfterX : MonoBehaviour
{
    public float timeToDestroy = 1f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}

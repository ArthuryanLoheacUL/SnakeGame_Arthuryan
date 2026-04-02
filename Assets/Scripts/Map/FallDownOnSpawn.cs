using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FallDownOnSpawn : MonoBehaviour
{
    [SerializeField] private GameObject impactParticulesPrefab;

    private Vector3 targetPos;
    private SpriteRenderer spriteRenderer;
    private bool isFalling = false;
    private float lerpTime = 0f;
    private float maxLerpTime = 1f;
    private bool shaked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        shaked = false;
        if (GetComponent<ShadowCaster2D>() != null)
            GetComponent<ShadowCaster2D>().enabled = false;
        StartCoroutine(WaitAndFall(Random.Range(0, 11) / 100f));
    }

    System.Collections.IEnumerator WaitAndFall(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        isFalling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > targetPos.y && isFalling)
        {
            lerpTime += Time.deltaTime * 2;
            if (lerpTime > maxLerpTime)
            {
                lerpTime = maxLerpTime;
            }

            float _y = Mathf.Pow(lerpTime / maxLerpTime, 2);

            transform.position = new Vector3(transform.position.x,
                Mathf.Lerp(transform.position.y, targetPos.y, _y), transform.position.z);
            if (spriteRenderer != null)
            {
                Color _color = spriteRenderer.color;
                _color.a = 1 - (transform.position.y - targetPos.y);
                spriteRenderer.color = _color;
            }
        }
        if (!shaked && isFalling && Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            shaked = true;
            ShakeCameraManager.instance.ShakeCamera(0.1f, 0.1f, Vector2.down);
            Instantiate(impactParticulesPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            if (GetComponent<ShadowCaster2D>() != null)
                GetComponent<ShadowCaster2D>().enabled = true;
        }
    }
}

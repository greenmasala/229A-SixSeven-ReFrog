using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float FallDelay = 1f;
    public float DestroyDelay = 2f;
    public GameObject Layout;
    GameObject layoutPrefab;
    public string Tag;
    float delaySequence;
    public Vector3 CustomScale;
    public bool ApplyCustomScale;
    public GameObject[] AdditionalObjects;

    Rigidbody2D rb;
    SpriteRenderer color;

    private void Awake()
    {
        layoutPrefab = Instantiate(Layout, transform.position, Quaternion.identity);
        if (ApplyCustomScale)
        {
            layoutPrefab.transform.localScale = CustomScale;
        }
        layoutPrefab.tag = Tag;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        color = rb.GetComponent<SpriteRenderer>();
        delaySequence = FallDelay / 4;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
            Destroy(layoutPrefab);
            if (AdditionalObjects != null)
            {
                foreach (var item in AdditionalObjects)
                {
                    Destroy(item);
                }
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(delaySequence);
        color.color = Color.indianRed;
        yield return new WaitForSeconds(delaySequence);
        color.color = Color.orangeRed;
        yield return new WaitForSeconds(delaySequence);
        color.color = Color.red;
        yield return new WaitForSeconds(delaySequence);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, DestroyDelay);
    }
}

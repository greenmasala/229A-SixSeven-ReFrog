using System.Collections;
using UnityEngine;

public class RefreshPlatforms : MonoBehaviour
{
    public Transform[] MovePoints;
    public int MoveSpeed = 5;
    //Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        if (Refresh.Instance.HasRefreshed)
        {
            if (transform.position != MovePoints[1].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, MovePoints[1].position, MoveSpeed * Time.deltaTime);
            }
        }

        else
        {
            if (transform.position != MovePoints[0].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, MovePoints[0].position, MoveSpeed * Time.deltaTime);
                Debug.Log("running");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}


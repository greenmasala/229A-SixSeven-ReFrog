using DG.Tweening;
using UnityEngine;

public class RefreshPlatforms : MonoBehaviour
{
    public Transform[] MovePoints;
    
    // Update is called once per frame
    void Update()
    {
        if (Refresh.Instance.HasRefreshed)
        {
            if (transform.position != MovePoints[1].position)
            {
                transform.DOMove(MovePoints[1].position, 0.1f).SetEase(Ease.InOutQuad);
            }
        }

        else
        {
            if (transform.position != MovePoints[0].position)
            {
                transform.DOMove(MovePoints[0].position, 0.1f).SetEase(Ease.InOutQuad);
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


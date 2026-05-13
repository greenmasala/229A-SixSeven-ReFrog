using UnityEngine;

public class ReplenishRefresh : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Refresh.Instance.ReplenishRefresh();
        Destroy(gameObject);
    }
}

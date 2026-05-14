using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public Player Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Death);
            Player.Death();
        }
    }
}

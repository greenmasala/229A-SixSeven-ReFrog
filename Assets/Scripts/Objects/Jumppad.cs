using JetBrains.Annotations;
using UnityEngine;

public class Jumppad : MonoBehaviour
{
    public int JumpForce = 15;
    Animator JumpPadAnim;

    void Start()
    {
        JumpPadAnim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JumpPadAnim.SetBool("IsJumping", true);
            collision.gameObject.GetComponent<Player>().JumpFX();
            collision.gameObject.GetComponent<Player>().JumpCount = 1;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Jumppad);
            var force = collision.rigidbody.mass * JumpForce;
            collision.rigidbody.linearVelocity = new Vector2(collision.rigidbody.linearVelocity.x, 0f);
            collision.rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            JumpPadAnim.SetBool("IsJumping", false);
        }
    }
}


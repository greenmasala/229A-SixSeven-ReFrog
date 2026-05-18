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
            collision.gameObject.GetComponent<Player>().Jumppad();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Jumppad);
            var force = collision.rigidbody.mass * JumpForce; //this part is for physics assignment!
            collision.rigidbody.linearVelocity = new Vector2(collision.rigidbody.linearVelocity.x, 0f);
            collision.rigidbody.AddForce(force * Vector2.up, ForceMode2D.Impulse); //AddForce(JumpForce * Vector2.up, ForceMode2D.Impulse)
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


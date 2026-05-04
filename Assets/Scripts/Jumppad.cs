using UnityEngine;

public class Jumppad : MonoBehaviour
{
    public int JumpForce = 15;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var force = collision.rigidbody.mass * JumpForce;
            collision.rigidbody.linearVelocity = new Vector2(collision.rigidbody.linearVelocity.x, 0f);
            collision.rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }
}

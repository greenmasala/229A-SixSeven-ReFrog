using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int speed = 10;
    [SerializeField] int jumpForce = 10;
    public int JumpCount = 1;
    int maxJumpCount;

    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter = 0f;

    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter;
    public ParticleSystem DustFX;

    public bool isJumping;
    public Transform GroundCheckPos;
    public Vector2 GroundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask GroundLayer;
    bool facingRight;
    Animator playerAnim;

    public int RefreshCount = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxJumpCount = JumpCount;
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            if (isGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else if (!isGrounded())
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBufferCounter = jumpBufferTime;
                if (coyoteTimeCounter < 0 & JumpCount > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    JumpSFX();
                    DustFX.Play();
                    isJumping = true;
                    JumpCount = 0;
                }
                else if (coyoteTimeCounter > 0 & JumpCount > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    DustFX.Play();
                    JumpSFX();
                }
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (isJumping)
            {
                playerAnim.SetBool("IsJumping", true);
            }
            else if (!isJumping)
            {
                playerAnim.SetBool("IsJumping", false);
            }

            if (jumpBufferCounter > 0f) 
            {
                if (coyoteTimeCounter > 0f && !isJumping)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    isJumping = true;
                    JumpSFX();
                    DustFX.Play();

                    jumpBufferCounter = 0f;
                    coyoteTimeCounter = 0f;
                }
                else if (isJumping)
                {
                    if (JumpCount > 0)
                    {
                        JumpCount--;
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                        JumpSFX();
                        DustFX.Play();
                        jumpBufferCounter = 0f;
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.Paused)
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");

            rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

            if (horizontalInput != 0)
            {
                playerAnim.SetBool("IsWalking", true);
            }
            else
            {
                playerAnim.SetBool("IsWalking", false);
            }

            if (horizontalInput < 0 && !facingRight)
            {
                Flip();
            }
            else if (horizontalInput > 0 && facingRight)
            {
                Flip();
            }

            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 1.25f * Time.deltaTime;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            playerAnim.SetBool("IsWalking", false);
            playerAnim.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Death();
        }
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(GroundCheckPos.position, GroundCheckSize, 0, GroundLayer) & rb.linearVelocity.y <= 0) 
        {
            JumpCount = maxJumpCount;
            isJumping = false;
            return true;
        }
        return false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        if (!facingRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundCheckPos.position, GroundCheckSize);
    }

    public void Death()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Death);
        GameManager.Instance.Death(transform);
        Destroy(gameObject);
    }
    void WalkSFX() //used in player walk animation clip
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Walk);
    }

    public void Jumppad()
    {
        isJumping = true;
        DustFX.Play();
        JumpCount = 1;
    }

    void JumpSFX()
    {
        int no = Random.Range(0, 670);
        if (no == 67) //this is just for funsies
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Yahoo);
        }
        else
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Jump);
        }
    }
}

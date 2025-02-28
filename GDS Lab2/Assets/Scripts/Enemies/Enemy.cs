using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public enum Direction { Left, Right }
    public Direction startingDirection = Direction.Right; // Default starting direction

    public float speed = 1f;
    protected Vector2 movementDirection;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private bool movementEnabled = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Set initial movement direction
        movementDirection = (startingDirection == Direction.Left) ? Vector2.left : Vector2.right;
    }

    protected virtual void FixedUpdate()
    {
        if (movementEnabled)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        movementEnabled = isEnabled;
        if (!movementEnabled)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fire") || collision.gameObject.CompareTag("Shell Moving"))
        {
            HitSequence();
        }
        else if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Object hit!");

            movementDirection = (movementDirection == Vector2.left) ? Vector2.right : Vector2.left;
        }
        else
        {
            // Check if the player is landing on top
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.5f) // Player is above the enemy
            {
                HandlePlayerStomp(collision);
            }
            if(collision.gameObject.GetComponent<MarioState>().isInvincible){ HitSequence(); }
        }
    }

    protected virtual void HitSequence()
    {
        SetMovementEnabled(false);

        rb.isKinematic = false;
        rb.gravityScale = 1f;
        animator.enabled = false;
        spriteRenderer.flipY = true;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        rb.linearVelocity = new Vector2(0f, 3f); 
        Destroy(gameObject, 2f);   
    }

    protected abstract void HandlePlayerStomp(Collision2D collision);
}
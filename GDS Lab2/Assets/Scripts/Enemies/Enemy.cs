using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public enum Direction { Left, Right }
    public Direction startingDirection = Direction.Right; // Default starting direction

    public float speed = 1f;
    private Vector2 movementDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure Rigidbody2D is dynamic for collision detection
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Set initial movement direction
        movementDirection = (startingDirection == Direction.Left) ? Vector2.left : Vector2.right;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected void Move()
    {
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            // Enemy dies on fire collision
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Object hit!");

            // Reverse movement direction on collision
            movementDirection = (movementDirection == Vector2.left) ? Vector2.right : Vector2.left;
        }
        else
        {
            // Check if the player is landing on top
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.5f) // Player is above the enemy
            {
                HandlePlayerStomp(collision);

                // Make the player bounce up
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 5f); // Adjust bounce height as needed
                }
            }
        }
    }

    protected abstract void HandlePlayerStomp(Collision2D collision);
}
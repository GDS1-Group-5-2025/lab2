using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    public enum Direction { Left, Right }
    public Direction startingDirection = Direction.Right; // Default starting direction

    private Vector3 _startingPosition;

    public float speed = 1f;
    protected Vector2 movementDirection;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private bool _movementEnabled;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Set initial movement direction
        movementDirection = (startingDirection == Direction.Left) ? Vector2.left : Vector2.right;

        _startingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    protected virtual void FixedUpdate()
    {
        if (_movementEnabled)
        {
            Move();
        }
        if(isDead && deadTimer <= 0){
            gameObject.SetActive(false);
        }
        else if(isDead){
            deadTimer -= Time.deltaTime;
        }
    }

    protected virtual void Move()
    {
        rb.MovePosition(rb.position + movementDirection * (speed * Time.fixedDeltaTime));
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _movementEnabled = isEnabled;
        if (!_movementEnabled)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Fire or moving shell
        if (collision.gameObject.CompareTag("Fire") || collision.gameObject.CompareTag("Shell Moving"))
        {
            HitSequence();
            return;
        }

        // If collision is not with player or floor, change direction
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Floor"))
        {
            movementDirection = (movementDirection == Vector2.left) ? Vector2.right : Vector2.left;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            return;
        }

        // Collided object is player
        var marioState = collision.gameObject.GetComponent<MarioState>();
        if (marioState == null) return;

        var contact = collision.GetContact(0);
        // Stomp
        if (contact.normal.y < -0.5f)
        {
            HandlePlayerStomp(collision);

            var marioMovement = collision.gameObject.GetComponent<MarioMovement>();
            if (marioMovement != null)
            {
                marioMovement.Stomp();
            }

            AudioManager.Instance.PlaySFX("stomp");
        }

        // Mario takes damage
        else
        {
            if (marioState.isInvincible)
            {
                // Mario kills enemy on contact
                HitSequence();
            }
            else
            {
                // Mario takes damage
                marioState.TakeDamage();
            }
        }
    }

    private bool isDead = false;
    private float deadTimer = 0;
    protected virtual void HitSequence()
    {
        SetMovementEnabled(false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        animator.enabled = false;
        spriteRenderer.flipY = true;

        var colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        rb.linearVelocity = new Vector2(0f, 3f);
        isDead = true; deadTimer = 3;
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        isDead = false;
        transform.position = _startingPosition;
        SetMovementEnabled(false);
        rb.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.flipY = false;
        animator.enabled = true;
        spriteRenderer.enabled = true;
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = true;
        }
    }

    protected abstract void HandlePlayerStomp(Collision2D collision);

}

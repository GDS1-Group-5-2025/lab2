using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public enum Direction { Left, Right }
    public Direction startingDirection = Direction.Right; //default starting direction

    public float speed = 1f;
    private Vector3 movementDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //ensure Rigidbody is dynamic for collision detection
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        //set initial movement direction
        movementDirection = (startingDirection == Direction.Left) ? Vector3.left : Vector3.right;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected void Move()
    {
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall hit!");

            //reverse movement direction on collision
            movementDirection = (movementDirection == Vector3.left) ? Vector3.right : Vector3.left;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < -0.5f) //player above enemy
            {
                HandlePlayerStomp(collision); //handle stomp differently for each enemy
                
                // >>>>> make player bounce up <<<<<
            }
        }
        if (collision.gameObject.CompareTag("Fire"))
        {
            //enemy dies on fire collision
            Destroy(gameObject);
        }
    }

    protected abstract void HandlePlayerStomp(Collision collision);
}
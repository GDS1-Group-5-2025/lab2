using UnityEngine;

public class Enemy : MonoBehaviour
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

    void FixedUpdate()
    {
        //move using Rigidbody to respect collisions
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall hit!");

            //reverse movement direction on collision
            movementDirection = (movementDirection == Vector3.left) ? Vector3.right : Vector3.left;
        }
    }
}
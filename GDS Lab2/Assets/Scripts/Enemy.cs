using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Direction { Left, Right }
    public Direction startingDirection = Direction.Right; //default direction is right unless changed in the inspector

    private float speed = 1f;
    private Vector2 MovementDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovementDirection = (startingDirection == Direction.Left) ? Vector2.left : Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(MovementDirection * speed * Time.deltaTime);
    }
}

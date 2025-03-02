using UnityEngine;

internal enum BrickBlockState // Enum for the state of the block
{
    Normal, // Normal state
    Exploded // Hit state
}

public class BrickBlock : MonoBehaviour
{
    private static readonly int Bump = Animator.StringToHash("Bump");
    private static readonly int Explode = Animator.StringToHash("Explode");

    public Animator animator;

    private BrickBlockState _state = BrickBlockState.Normal;

    private MarioState _marioState;

    private void Start()
    {
        _marioState = FindFirstObjectByType<MarioState>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the block is already hit, return
        if (_state == BrickBlockState.Exploded) return;
        // If the collision is with the player
        if (!collision.gameObject.CompareTag("Player")) return;

        if (_marioState.currentState == MarioStateEnum.Small)
        {
            animator.SetTrigger(Bump);
        }
        else
        {
            animator.SetTrigger(Explode);
            _state = BrickBlockState.Exploded;
        }
    }

    public void Reset()
    {
        // Set the state to "Normal"
        _state = BrickBlockState.Normal;
        // Reset the animator
        animator.Rebind();
    }
}

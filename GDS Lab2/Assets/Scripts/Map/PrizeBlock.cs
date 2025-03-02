using System.Collections;
using UnityEngine;

internal enum PrizeBlockState // Enum for the state of the block
{
    Normal, // Normal state
    Hit // Hit state
}

// PrizeBlock is a class that represents a block that contains a prize and is consumed after being hit by any type of player
public class PrizeBlock : MonoBehaviour
{
    private static readonly int Hit = Animator.StringToHash("Hit");

    // Item that will be spawned when the block is hit
    public GameObject item;
    public Animator animator;

    private PrizeBlockState _state = PrizeBlockState.Normal;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the block is already hit, return
        if (_state == PrizeBlockState.Hit) return;
        // If the collision is with the player
        if (!collision.gameObject.CompareTag("Player")) return;

        // Trigger the "Hit" trigger
        animator.SetTrigger(Hit);
        // Set the state to "Hit"
        _state = PrizeBlockState.Hit;

        // Instantiate the item
        if (item)
        {
            Instantiate(item, transform.position, Quaternion.identity);
            try{ item.gameObject.GetComponent<BoxCollider2D>().enabled = false; }
            catch{}
            try{ item.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0; }
            catch{}
        }
    }

    public void Reset()
    {
        // Set the state to "Normal"
        _state = PrizeBlockState.Normal;
        // Reset the animator
        animator.Rebind();
    }
}

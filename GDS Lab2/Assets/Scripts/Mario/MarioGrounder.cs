using UnityEngine;

public class MarioGrounder : MonoBehaviour
{
    private MarioMovement _marioMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _marioMovement = GetComponentInParent<MarioMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            _marioMovement.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            _marioMovement.isGrounded = false;
        }
    }
}

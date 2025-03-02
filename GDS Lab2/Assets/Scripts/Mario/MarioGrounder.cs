using UnityEngine;

public class MarioGrounder : MonoBehaviour
{
    private MarioMovement _marioMovement;
    private int _groundedCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _marioMovement = GetComponentInParent<MarioMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            _marioMovement.isGrounded = true;
            _groundedCount++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _groundedCount--;
        if (_groundedCount == 0)
        {
            _marioMovement.isGrounded = false;
        }
    }
}

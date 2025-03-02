using UnityEngine;

public class TriggerEnemies : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>()?.SetMovementEnabled(true);
        }
    }
}

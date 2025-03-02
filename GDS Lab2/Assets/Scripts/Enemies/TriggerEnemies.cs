using UnityEngine;

public class TriggerEnemies : MonoBehaviour
{

    public Enemy[] enemies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in enemies)
            {
                enemy.SetMovementEnabled(true);
            }
        }
    }
}

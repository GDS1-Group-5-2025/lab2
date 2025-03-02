using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Enemy[] enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = GetComponentsInChildren<Enemy>();
    }

    public void ResetEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.Reset();
        }
    }
}

using UnityEngine;

public class Goomba : Enemy
{
    private void Die()
    {
        Destroy(gameObject);
    }

    protected override void HandlePlayerStomp(Collision2D collision)
    {
        Die();
    }
}

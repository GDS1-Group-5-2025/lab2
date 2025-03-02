using UnityEngine;
using System.Collections;

public class Goomba : Enemy
{
    public Sprite stompSprite;

    private void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = stompSprite;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    protected override void HandlePlayerStomp(Collision2D collision)
    {
        SetMovementEnabled(false);

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        Die();
    }
}

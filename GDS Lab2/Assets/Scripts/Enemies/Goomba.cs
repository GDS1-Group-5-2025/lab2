using UnityEngine;
using System.Collections;

public class Goomba : Enemy
{
    public Sprite stompSprite;
    private Sprite _startingSprite;

    private new void Start()
    {
        base.Start();
        _startingSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        animator.enabled = false;
        GetComponent<SpriteRenderer>().sprite = stompSprite;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
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

    protected new void Reset()
    {
        base.Reset();
        SetMovementEnabled(true);
        GetComponent<SpriteRenderer>().sprite = _startingSprite;
        animator.enabled = true;
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = true;
        }
    }
}

using UnityEngine;

public class KoopaTroopa : Enemy
{
    private enum KoopaState { Walking, Shell, ShellMoving }
    private KoopaState currentState = KoopaState.Walking;

    [SerializeField] private Sprite shellSprite;

    private SpriteRenderer spriteRenderer;
    //private CircleCollider2D circleCollider;
    private PolygonCollider2D polygonCollider;

    private float originalSpeed;

    private void InitializeKoopa() 
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        //circleCollider = GetComponent<CircleCollider2D>();

        originalSpeed = speed;

        /*if (circleCollider != null)
            circleCollider.enabled = false;*/
    }

    protected override void HandlePlayerStomp(Collision2D collision)
    {
        if (currentState == KoopaState.Walking)
        {
            ShellMode();
        }
        else if (currentState == KoopaState.Shell)
        {
            KickShell(collision);
        }
    }

    private void ShellMode()
    {
        currentState = KoopaState.Shell;

        if (shellSprite != null)
        {
            spriteRenderer.sprite = shellSprite;
        }

        if (polygonCollider != null) polygonCollider.enabled = false;
        //if (circleCollider != null) circleCollider.enabled = true;

        speed = 0;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;

        Debug.Log("Koopa entered shell mode!");
    }

    private void KickShell(Collision2D collision)
    {
        currentState = KoopaState.ShellMoving;
        speed = originalSpeed * 2;

        float playerDirection = Mathf.Sign(collision.transform.position.x - transform.position.x);
        movementDirection = new Vector2(playerDirection, 0);

        Debug.Log("Koopa shell kicked!");
    }
}
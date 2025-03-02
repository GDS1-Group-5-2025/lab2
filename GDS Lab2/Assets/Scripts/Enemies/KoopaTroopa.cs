using UnityEngine;
using System.Collections;

public class KoopaTroopa : Enemy
{
    private enum KoopaState { Walking, Shell, ShellMoving }
    private KoopaState _currentState = KoopaState.Walking;

    [SerializeField] private Sprite shellSprite;

    private CircleCollider2D _circleCollider;
    private BoxCollider2D _boxCollider;

    private float _originalSpeed;

    private void InitializeKoopa()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _circleCollider = GetComponent<CircleCollider2D>();

        _originalSpeed = speed;

        if (_circleCollider)
        {
            _circleCollider.enabled = false;
        }
    }

    protected override void Start()
    {
        base.Start();
        InitializeKoopa();
    }

    protected override void HandlePlayerStomp(Collision2D collision)
    {
        if (_currentState == KoopaState.Walking)
        {
            ShellMode();
        }
        /*if (currentState == KoopaState.Shell)
        {
            KickShell(collision);
        }*/

        // The above code section was just for testing if kicking works
    }

    protected override void Move()
    {
        base.Move();

        if (spriteRenderer)
        {
            spriteRenderer.flipX = movementDirection.x > 0;
        }
    }

    private void ShellMode()
    {
        _currentState = KoopaState.Shell;

        if (shellSprite != null)
        {
            spriteRenderer.sprite = shellSprite;
        }

        if (animator != null)
        {
            animator.enabled = false;
        }

        if (_boxCollider != null) _boxCollider.enabled = false;
        if (_circleCollider != null) _circleCollider.enabled = true;

        SetMovementEnabled(false);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;

        gameObject.tag = "Shell";
        Debug.Log("Koopa entered shell mode!");

        StartCoroutine(ShellTimer());
    }

    private IEnumerator ShellTimer()
    {
        yield return new WaitForSeconds(13);
        if (_currentState == KoopaState.Shell)
        {
            ExitShellMode();
        }
    }

    private void ExitShellMode()
    {
        _currentState = KoopaState.Walking;

        if (animator )
        {
            animator.enabled = true;
        }

        if (_boxCollider ) _boxCollider.enabled = true;
        if (_circleCollider) _circleCollider.enabled = false;

        speed = _originalSpeed;

        SetMovementEnabled(true);

        gameObject.tag = "Koopa";
        Debug.Log("Koopa exited shell mode!");
    }

    //This function should be called by Mario when he is next to a shell and presses jump button to kick it
    public void KickShell(Collision2D collision)
    {
        _currentState = KoopaState.ShellMoving;
        speed = _originalSpeed * 4;

        //float playerDirection = Mathf.Sign(collision.transform.position.x - transform.position.x);
        //movementDirection = new Vector2(playerDirection, 0);

        //Above code for testing

        if (rb)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        SetMovementEnabled(true);

        gameObject.tag = "Shell Moving";

        Debug.Log("Koopa shell kicked!");
    }

    protected override void HitSequence()
    {
        base.HitSequence();
        spriteRenderer.sprite = shellSprite;
    }
}

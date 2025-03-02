using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MarioAnimatorBridge : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private MarioMovement _movement;
    private MarioState _state;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _movement = GetComponent<MarioMovement>();
        _state = GetComponent<MarioState>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float speedAbs = Mathf.Abs(_movement.newXSpeedInSmwUnits);

        // Update animator parameters
        _anim.SetFloat("Speed", speedAbs);
        _anim.SetBool("IsGrounded", _movement.isGrounded);

        bool actuallyJumping = _movement.isJumping && !_movement.isGrounded;
        _anim.SetBool("IsJumping", actuallyJumping);

        _anim.SetBool("IsDead", _state.currentState == MarioStateEnum.Dead);

        if (Mathf.Abs(_movement.newXSpeedInSmwUnits) > 0.01f)
        {
            _spriteRenderer.flipX = (_movement.newXSpeedInSmwUnits < 0);
        }
    }
}
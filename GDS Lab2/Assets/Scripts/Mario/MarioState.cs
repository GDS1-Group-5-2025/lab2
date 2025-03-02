using UnityEngine;

public enum MarioStateEnum
{
    Small,
    Big,
    Fire,
    Dead
}

public class MarioState : MonoBehaviour
{
    public MarioStateEnum currentState = MarioStateEnum.Small;
    public bool isInvincible;
    [SerializeField] private float invincibleDuration;
    private bool wasInvincible;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isInvincible && !wasInvincible)
        {
            MusicManager.Instance.StartInvincibility();
        }

        if (isInvincible)
        {
            invincibleDuration -= Time.deltaTime;
            if (invincibleDuration <= 0f)
            {
                isInvincible = false;
                MusicManager.Instance.StopInvincibility();
            }
        }

        wasInvincible = isInvincible;

        if (_animator != null && currentState == MarioStateEnum.Dead)
        {
            _animator.SetBool("IsDead", true);
        } 
        else 
        {
            _animator.SetBool("IsDead", false);
        }
    }

    public MarioStateEnum TakeDamage()
    {
        if (isInvincible) return currentState;

        currentState = currentState switch
        {
            MarioStateEnum.Small => MarioStateEnum.Dead,
            MarioStateEnum.Big => MarioStateEnum.Small,
            MarioStateEnum.Fire => MarioStateEnum.Big,
            _ => currentState
        };

        if (currentState == MarioStateEnum.Dead)
        {
            if (MarioLifeSystem.Instance != null)
            {
                MarioLifeSystem.Instance.HandleMarioDeath();
            }
            else
            {
                Debug.Log("MarioLifeSystem not found");
            }
        }

        return currentState;
    }

    public void PowerUp()
    {
        currentState = currentState switch
        {
            MarioStateEnum.Small => MarioStateEnum.Big,
            MarioStateEnum.Big => MarioStateEnum.Fire,
            _ => currentState
        };
    }

    public void SetIsInvincible(bool ans){
        isInvincible = ans;

        if (isInvincible)
        {
            invincibleDuration = 30f;
            Debug.Log("Invincible!");
        }
        else
        {
            invincibleDuration = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ItemData>().GetItemName() == "PowerUp")
        {
            PowerUp();
            Destroy(collision.gameObject);
            AudioManager.Instance.PlaySFX("powerup");
        }
    }
}

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

    void Update()
    {
        if (isInvincible){
            invincibleDuration -= Time.deltaTime;
            if(invincibleDuration <= 0){ isInvincible = false; }
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
            AudioManager.Instance.PlaySFX("death");
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
        invincibleDuration = 30f;
    }
}

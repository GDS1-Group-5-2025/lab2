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
}

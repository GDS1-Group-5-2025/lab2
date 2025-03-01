using UnityEngine;

[RequireComponent(typeof(MarioState))]
public class MarioLifeSystem : MonoBehaviour
{
    public static MarioLifeSystem Instance;

    private MarioState _marioState;

    public int maxLives = 3;
    public Vector2 respawnPosition = new Vector2(3.4f, 1f);

    private int livesRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        _marioState = GetComponent<MarioState>();
    }

    void Start()
    {
        livesRemaining = maxLives;
    }

    public void HandleMarioDeath()
    {
        livesRemaining--;

        if (livesRemaining > 0)
        {
            RespawnMario();
        }
        else
        {
            GameOver();
        }
    }

    private void RespawnMario()
    {
        _marioState.currentState = MarioStateEnum.Small;
        _marioState.SetIsInvincible(false);

        transform.position = respawnPosition;
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER!");
        //trigger game over UI
    }
}

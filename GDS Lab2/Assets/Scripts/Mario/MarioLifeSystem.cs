using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MarioState))]
public class MarioLifeSystem : MonoBehaviour
{
    public static MarioLifeSystem Instance;

    private MarioState _marioState;
    private PlayerInput _playerInput;

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
        _playerInput = GetComponent<PlayerInput>();
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
            StartCoroutine(RespawnMario());
        }
        else
        {
            GameOver();
        }
    }

    private IEnumerator RespawnMario()
    {
        MusicManager.Instance.PlayMusic("death");
        DisableUserInput();
        yield return new WaitForSeconds(2);

        _marioState.currentState = MarioStateEnum.Small;
        _marioState.SetIsInvincible(false);

        transform.position = respawnPosition;
        EnableUserInput();
    }

    private void DisableUserInput()
    {
        if (_playerInput != null)
        {
            _playerInput.enabled = false;
        }
    }

    private void EnableUserInput()
    {
        if (_playerInput != null)
        {
            _playerInput.enabled = true;
        }
    }

    private void GameOver()
    {
        MusicManager.Instance.PlayMusic("game over");
        Debug.Log("GAME OVER!");
        //trigger game over UI
    }
}

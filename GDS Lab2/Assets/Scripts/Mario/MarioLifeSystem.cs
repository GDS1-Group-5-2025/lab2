using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MarioState))]
public class MarioLifeSystem : MonoBehaviour
{
    public static MarioLifeSystem Instance;

    private MarioState _marioState;
    private PlayerInput _playerInput;

    public int maxLives = 3;
    public Vector2 respawnPosition = new Vector2(3.4f, 1f);

    public int livesRemaining;

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
            SceneManager.LoadScene("Loading Screen");
            StartCoroutine(RespawnMario());
        }
        else
        {
            GameOver();
        }
    }

    private IEnumerator RespawnMario()
    {
        MusicManager.Instance.PlayNonLoopingClipThenRevert("death");
        DisableUserInput();
        yield return new WaitForSeconds(3);

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
        MusicManager.Instance.PlayNonLoopingClipThenRevert("game over");
        Debug.Log("GAME OVER!");
        //trigger game over UI
    }
}

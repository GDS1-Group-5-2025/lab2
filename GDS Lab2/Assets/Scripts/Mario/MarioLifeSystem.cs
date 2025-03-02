using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UIElements;

[RequireComponent(typeof(MarioState))]
public class MarioLifeSystem : MonoBehaviour
{
    public static MarioLifeSystem Instance;

    private MarioState _marioState;
    private PlayerInput _playerInput;

    public int maxLives = 3;
    public Transform respawnPosition;

    private int _livesRemaining;

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
        _livesRemaining = maxLives;
    }

    public void HandleMarioDeath()
    {
        _livesRemaining--;

        if (_livesRemaining > 0)
        {
            SceneManager.LoadScene("Loading Screen");
            StartCoroutine(RespawnMario());
        }
        else
        {
            isGameOver = true;
            GameOverManager.Instance.GameOver();
        }
    }

    private IEnumerator RespawnMario()
    {
        if (isGameOver) yield break;

        MusicManager.Instance.PlayNonLoopingClipThenRevert("death");
        DisableUserInput();
        yield return new WaitForSeconds(3);

        _marioState.currentState = MarioStateEnum.Small;
        _marioState.SetIsInvincible(false);

        transform.position = respawnPosition.position;
        EnableUserInput();
    }

    private void DisableUserInput()
    {
        if (_playerInput)
        {
            _playerInput.enabled = false;
        }
    }

    private void EnableUserInput()
    {
        if (_playerInput )
        {
            _playerInput.enabled = true;
        }
    }
}

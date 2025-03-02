using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MarioState))]
public class MarioLifeSystem : MonoBehaviour
{
    public static MarioLifeSystem Instance;

    private MarioState _marioState;
    private PlayerInput _playerInput;

    private GameObject _camera;

    public int maxLives = 3;
    public Transform respawnPosition;
    public Transform cameraRespawnPosition;

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
        _camera = Camera.main?.gameObject;
    }

    public void HandleMarioDeath()
    {
        _livesRemaining--;

        if (_livesRemaining > 0)
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
        MusicManager.Instance.PlayNonLoopingClipThenRevert("death");
        DisableUserInput();
        yield return new WaitForSeconds(2);

        _marioState.currentState = MarioStateEnum.Small;
        _marioState.SetIsInvincible(false);

        transform.position = respawnPosition.position;
        if (_camera )
            _camera.transform.position = cameraRespawnPosition.position;
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

    private void GameOver()
    {
        MusicManager.Instance.PlayNonLoopingClipThenRevert("game over");
        Debug.Log("GAME OVER!");
        //trigger game over UI
    }
}

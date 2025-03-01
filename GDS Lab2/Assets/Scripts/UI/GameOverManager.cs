using UnityEngine;
using UnityEngine.Events;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    public UnityEvent OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        MusicManager.Instance.PlayNonLoopingClipThenRevert("game over");
        Debug.Log("GAME OVER!");

        // Invoke the OnGameOver event
        if (OnGameOver != null)
        {
            OnGameOver.Invoke();
        }
    }
}
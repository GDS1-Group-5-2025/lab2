using UnityEngine;
using UnityEngine.Events;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    public UnityEvent OnGameOver;

    public int livesRemaining = 3;

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

        // Find the Timer component in the TimeManager GameObject and stop the timer
        GameObject timeManager = GameObject.Find("TimeManager");
        if (timeManager != null)
        {
            Timer timer = timeManager.GetComponent<Timer>();
            if (timer != null)
            {
                timer.StopTimer();
            }
        }

        // Invoke the OnGameOver event
        if (OnGameOver != null)
        {
            OnGameOver.Invoke();
        }
    }
}
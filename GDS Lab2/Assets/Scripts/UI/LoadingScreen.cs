using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public float delayBeforeLoadingMainScene = 3.0f;
    public TextMeshProUGUI livesText; 

    void Start()
    {
        Debug.Log("Loading Main Level after " + delayBeforeLoadingMainScene + " seconds");

        // Display lives if GameOverManager exists
        if (GameOverManager.Instance != null)
        {
            livesText.text = GameOverManager.Instance.livesRemaining.ToString();
        }
        else
        {
            livesText.text = "3";  
        }

        StartCoroutine(LoadMainLevelAfterDelay());
    }

    private IEnumerator LoadMainLevelAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoadingMainScene);

        SceneManager.LoadScene("Main Level");
    }
}

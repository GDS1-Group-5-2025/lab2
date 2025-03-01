using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float delayBeforeLoadingMainScene = 2.0f; 

    void Start()
    {
        StartGame();
    }

    public void LoadGame()
    {
        Debug.Log("LoadGame method called");
        SceneManager.LoadScene("Loading Screen");
        Debug.Log("Loading Screen Loaded");
    }

    public void StartGame()
    {
        Debug.Log("StartGame method called");
        StartCoroutine(LoadMainSceneAfterDelay());
    }

    private IEnumerator LoadMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoadingMainScene);

        SceneManager.LoadScene("Main Level");
        Debug.Log("Main Level Loaded");
    }
}
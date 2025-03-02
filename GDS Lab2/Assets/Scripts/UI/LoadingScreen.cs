using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public float delayBeforeLoadingMainScene = 2.0f;

    void Start()
    {
        Debug.Log("Loading Main Level after " + delayBeforeLoadingMainScene + " seconds");
        StartCoroutine(LoadMainLevelAfterDelay());
    }

    private IEnumerator LoadMainLevelAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoadingMainScene);

        SceneManager.LoadScene("Main Level"); 
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float delayBeforeLoadingMainLevel = 2.0f;

    public void LoadGame()
    {
        SceneManager.LoadScene("Loading Screen");
    }

}

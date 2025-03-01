using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Loading Screen");
        Debug.Log("Game Loaded");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Level");
        Debug.Log("Game Started");
    }
}
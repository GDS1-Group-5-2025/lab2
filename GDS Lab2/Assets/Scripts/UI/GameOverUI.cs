using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public Image gameOverImage;
    public TextMeshProUGUI gameOverText;

    void Start()
    {      
        HideGameOverScreen();

        // Subscribe to the GameOver event
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.OnGameOver.AddListener(ShowGameOverScreen);
        }
    }

    public void ShowGameOverScreen()
    {       
        gameOverImage.color = new Color(0, 0, 0, 1);
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 1);
    }

    public void HideGameOverScreen()
    {

        gameOverImage.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 0);
    }
}
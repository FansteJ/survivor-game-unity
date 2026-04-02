using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance { get; private set; }

    public Canvas canvas;
    public TMP_Text levelText;
    public TMP_Text durationText;
    public TMP_Text enemiesText;
    public TMP_Text coinsText;
    public Button mainMenuButton;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void ShowGameOver()
    {
        canvas.enabled = true;
        levelText.SetText("Level reached: " + ExperienceManager.Instance.CurrentLevel);
        float duration = GameManager.Instance.Duration;
        durationText.SetText("Run duration: " + $"{(int) duration/60}:{(int) duration%60:D2}");
        enemiesText.SetText("Enemies killed: " + GameManager.Instance.TotalEnemiesKilled);
        coinsText.SetText("Coins collected: " + CoinManager.Instance.TotalCoinsCollected);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    private void MainMenu()
    {
        Time.timeScale = 1;
        mainMenuButton.interactable = false;
        GameManager.Instance.SaveRunData();
    }
}

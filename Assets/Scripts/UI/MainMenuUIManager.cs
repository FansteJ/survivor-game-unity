using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text levelText;
    public TMP_Text xpText;
    public TMP_Text goldText;
    public TMP_Text gemsText;

    public Button playButton;
    public Button shopButton;
    public Button leaderboardButton;
    public Button logoutButton;

    public ShopUIManager shopUIManager;
    public LeaderboardUIManager leaderboardUIManager;
    void Start()
    {
        UserProfileDTO dto = ApiManager.Instance.CurrentProfile;
        FillUI(dto);
        playButton.onClick.AddListener(StartGame);
        shopButton.onClick.AddListener(() =>
        {
            shopUIManager.OpenShop();
        });
        leaderboardButton.onClick.AddListener(() =>
        {
            leaderboardUIManager.OpenLeaderboard();
        });
        logoutButton.onClick.AddListener(Logout);
    }

    private void FillUI(UserProfileDTO dto)
    {
        usernameText.SetText("Welcome " + dto.username);
        levelText.SetText(dto.level + "LVL");
        xpText.SetText(dto.currentXp + " / " + dto.neededXp);
        goldText.SetText(dto.gold + " Gold");
        gemsText.SetText(dto.gems + " Gems");
    }

    private void Logout()
    {
        ApiManager.Instance.Logout();
        SceneManager.LoadScene("Login");
    }

    private void StartGame()
    {
        GameSessionManager.Instance.StartGame(OnSuccessStartGame, OnErrorStartGame);
    }

    private void OnSuccessStartGame(string response)
    {
        PlayerManager.Instance.GetPlayerModifiers(
            onSuccess: (mods) =>
            {
                PlayerManager.Instance.activeModifiers = mods;

                SceneManager.LoadScene("Game");
            },
            onError: (error) =>
            {
                Debug.LogError("Error: " + error);
                SceneManager.LoadScene("Game");
            }
        );
    }

    private void OnErrorStartGame(string error)
    {
        Debug.LogError(error);
    }
}

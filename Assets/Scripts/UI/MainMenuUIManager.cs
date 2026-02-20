using TMPro;
using UnityEngine;
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


    void Start()
    {
        ProfileManager.Instance.GetProfile(OnSuccess, OnError);
        // playButton.onClick.AddListener();
        // shopButton.onClick.AddListener();
        // leaderboardButton.onClick.AddListener();
        // logoutButton.onClick.AddListener();
    }

    private void OnSuccess(UserProfileDTO dto)
    {
        usernameText.SetText("Welcome " + dto.username);
        levelText.SetText(dto.level + "LVL");
        xpText.SetText(dto.currentXp + " / " + dto.neededXp);
        goldText.SetText(dto.gold + " Gold");
        gemsText.SetText(dto.gems + " Gems");
    }

    private void OnError(string error)
    {
        Debug.Log("Error: " + error);
    }
}

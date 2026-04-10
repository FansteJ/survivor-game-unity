using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject leaderboardPanel;
    public GameObject leaderboardEntryPrefab;
    public Transform contentContainer;
    public MainMenuUIManager mainMenuManager;

    [Header("Main Menu")]
    public GameObject middleSection;
    public GameObject bottomBar;
    public GameObject quitButton;

    private List<GameObject> spawnedItems = new List<GameObject>();

    public void OpenLeaderboard()
    {
        middleSection.SetActive(false);
        bottomBar.SetActive(false);
        quitButton.SetActive(false);

        leaderboardPanel.SetActive(true);
        LoadLeaderboardItems();
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        middleSection.SetActive(true);
        bottomBar.SetActive(true);
        quitButton.SetActive(true);
    }

    private void LoadLeaderboardItems()
    {
        LeaderboardManager.Instance.GetLeaderboard(OnLeaderboardReceived, OnError);
    }

    private void OnLeaderboardReceived(List<LeaderboardEntryDTO> entries)
    {
        foreach (var item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();

        string myUsername = ApiManager.Instance.CurrentProfile.username;

        foreach (var entry in entries)
        {
            GameObject go = Instantiate(leaderboardEntryPrefab, contentContainer);
            LeaderboardEntryUI uiScript = go.GetComponent<LeaderboardEntryUI>();

            bool isMe = (entry.username == myUsername);
            uiScript.Setup(entry, isMe);

            spawnedItems.Add(go);
        }
    }

    private void OnError(string error)
    {
        Debug.LogError("Error: " + error);
    }
}
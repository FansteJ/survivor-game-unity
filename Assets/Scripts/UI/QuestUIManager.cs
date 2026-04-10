using System.Collections.Generic;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject questPanel;
    public GameObject questItemPrefab;
    public Transform contentContainer;
    public MainMenuUIManager mainMenuManager;

    [Header("Main Menu")]
    public GameObject middleSection;
    public GameObject bottomBar;
    public GameObject quitButton;

    private List<GameObject> spawnedItems = new List<GameObject>();
    private bool isLoading = false;

    public void OpenQuests()
    {
        middleSection.SetActive(false);
        bottomBar.SetActive(false);
        quitButton.SetActive(false);

        questPanel.SetActive(true);
        LoadQuests();
    }

    public void CloseQuests()
    {
        questPanel.SetActive(false);
        middleSection.SetActive(true);
        bottomBar.SetActive(true);
        quitButton.SetActive(true);
    }

    private void LoadQuests()
    {
        if(isLoading)
            { return; }
        isLoading = true;
        PlayerManager.Instance.GetDailyQuests(OnQuestsReceived, OnError);
    }

    private void OnQuestsReceived(List<UserQuestDTO> quests)
    {
        isLoading = false;
        foreach (var item in spawnedItems) Destroy(item);
        spawnedItems.Clear();

        foreach (var quest in quests)
        {
            GameObject go = Instantiate(questItemPrefab, contentContainer);
            QuestItemUI uiScript = go.GetComponent<QuestItemUI>();
            uiScript.Setup(quest, this);
            spawnedItems.Add(go);
        }
    }

    public void RequestClaimReward(string questId)
    {
        PlayerManager.Instance.ClaimQuestReward(questId, OnClaimSuccess, OnError);
    }

    private void OnClaimSuccess(string response)
    {
        Debug.Log("Quest claimed!");

        if (mainMenuManager != null)
        {
            mainMenuManager.RefreshUI();
        }

        LoadQuests();
    }

    private void OnError(string error)
    {
        isLoading = false;
        Debug.LogError("Error: " + error);
    }
}
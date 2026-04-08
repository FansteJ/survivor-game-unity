using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text progressText;
    public TMP_Text rewardText;
    public Button claimButton;

    private UserQuestDTO currentQuest;
    private QuestUIManager uiManager;

    public void Setup(UserQuestDTO quest, QuestUIManager manager)
    {
        currentQuest = quest;
        uiManager = manager;

        nameText.text = quest.questType.name;
        descriptionText.text = quest.questType.description;
        rewardText.text = $"+{quest.questType.rewardGems} Gems";

        progressText.text = $"{quest.progress} / {quest.questType.goal}";

        claimButton.onClick.RemoveAllListeners();

        if (quest.claimed)
        {
            claimButton.interactable = false;
            claimButton.GetComponentInChildren<TMP_Text>().text = "Claimed";
        }
        else if (quest.completed)
        {
            claimButton.interactable = true;
            claimButton.GetComponentInChildren<TMP_Text>().text = "Claim";
            claimButton.onClick.AddListener(OnClaimClicked);
        }
        else
        {
            claimButton.interactable = false;
            claimButton.GetComponentInChildren<TMP_Text>().text = "In Progress";
        }
    }

    private void OnClaimClicked()
    {
        claimButton.interactable = false;
        uiManager.RequestClaimReward(currentQuest.id);
    }
}
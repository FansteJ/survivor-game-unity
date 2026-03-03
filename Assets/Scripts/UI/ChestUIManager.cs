using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestUIManager : MonoBehaviour
{
    public static ChestUIManager Instance { get; private set; }

    public GameObject cardPrefab;
    public Transform cardContainer;

    public GameObject chestUI;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void ShowUpgradeCards(List<UpgradeOption> upgrades)
    {
        foreach (UpgradeOption upgradeOption in upgrades)
        {
            GameObject upgradeCard = Instantiate(cardPrefab, cardContainer);
            TMP_Text[] texts = upgradeCard.GetComponentsInChildren<TMP_Text>();
            texts[0].text = upgradeOption.name;
            texts[1].text = upgradeOption.description;
            UpgradeCard card = upgradeCard.AddComponent<UpgradeCard>();
            card.upgrade = upgradeOption;
            upgradeCard.GetComponent<Button>().onClick.AddListener(card.OnClick);
        }
    }

    public void ShowChest(List<UpgradeOption> upgrades)
    {
        Time.timeScale = 0;
        ShowUpgradeCards(upgrades);
        chestUI.SetActive(true);
    }

    public void HideChest()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        chestUI.SetActive(false);
        Time.timeScale = 1;
    }

}

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

    public bool IsUIOpen { get; private set; }

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

            Image cardImage = upgradeCard.GetComponent<Image>();
            cardImage.color = GetRarityColor(upgradeOption.rarity);

            UpgradeCard card = upgradeCard.AddComponent<UpgradeCard>();
            card.upgrade = upgradeOption;
            upgradeCard.GetComponent<Button>().onClick.AddListener(card.OnClick);
        }
    }

    public void ShowChest(List<UpgradeOption> upgrades)
    {
        Time.timeScale = 0;
        IsUIOpen = true;
        ShowUpgradeCards(upgrades);
        chestUI.SetActive(true);
    }

    public void HideChest()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        IsUIOpen = false;

        chestUI.SetActive(false);
        Time.timeScale = 1;
    }

    private Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return new Color(0.4f, 0.4f, 0.4f, 0.85f);
            case Rarity.Uncommon: return new Color(0.1f, 0.5f, 0.1f, 0.85f);
            case Rarity.Rare: return new Color(0.1f, 0.3f, 0.7f, 0.85f);
            case Rarity.Epic: return new Color(0.5f, 0.1f, 0.7f, 0.85f);
            case Rarity.Legendary: return new Color(0.8f, 0.6f, 0.0f, 0.85f);
            default: return Color.gray;
        }
    }
}

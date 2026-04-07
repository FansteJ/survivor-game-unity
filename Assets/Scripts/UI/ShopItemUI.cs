using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text descriptionText;
    public Button buyButton;
    public TMP_Text buyButtonText;

    private string upgradeId;
    private ShopUIManager shopManager;

    public void Setup(UpgradeShopItemDTO item, ShopUIManager manager)
    {
        shopManager = manager;
        upgradeId = item.upgradeTypeId;

        nameText.text = item.name;

        if (item.level >= item.maxLevel)
        {
            levelText.text = "MAX LEVEL";
            buyButtonText.text = "MAX";
            buyButtonText.color = Color.yellow;
            buyButton.interactable = false;
        }
        else
        {
            levelText.text = $"Lvl {item.level} -> {item.level + 1}";
            buyButtonText.text = $"{item.cost} Gold";
            buyButton.interactable = item.canBuy;

            if (item.canBuy)
            {
                buyButtonText.color = Color.green;
            }
            else
            {
                buyButtonText.color = Color.red;
            }
        }
        descriptionText.text = item.description;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnBuyClicked()
    {
        buyButton.interactable = false;
        shopManager.RequestBuyUpgrade(upgradeId);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopPanel;
    public GameObject shopItemPrefab;
    public Transform contentContainer;  
    public MainMenuUIManager mainMenuManager;

    [Header("Main Menu")]
    public GameObject middleSection;
    public GameObject bottomBar;
    public GameObject quitButton;

    private List<GameObject> spawnedItems = new List<GameObject>();

    public void OpenShop()
    {
        middleSection.SetActive(false);
        bottomBar.SetActive(false);
        quitButton.SetActive(false);

        shopPanel.SetActive(true);
        LoadShopItems();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);

        middleSection.SetActive(true);
        bottomBar.SetActive(true);
        quitButton.SetActive(true);
    }

    private void LoadShopItems()
    {
        PlayerManager.Instance.GetShopItems(OnShopItemsReceived, OnError);
    }

    private void OnShopItemsReceived(List<UpgradeShopItemDTO> items)
    {
        foreach (var item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();

        foreach (var item in items)
        {
            GameObject go = Instantiate(shopItemPrefab, contentContainer);
            ShopItemUI uiScript = go.GetComponent<ShopItemUI>();
            uiScript.Setup(item, this);
            spawnedItems.Add(go);
        }
    }

    public void RequestBuyUpgrade(string id)
    {
        PlayerManager.Instance.BuyUpgrade(id, OnBuySuccess, OnError);
    }

    private void OnBuySuccess(BuyUpgradeResponse response)
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.goldText.SetText(response.remainingGold + " Gold");
            mainMenuManager.gemsText.SetText(response.remainingGems + " Gems");
        }

        LoadShopItems();
    }

    private void OnError(string error)
    {
        Debug.LogError("Error: " + error);
    }
}
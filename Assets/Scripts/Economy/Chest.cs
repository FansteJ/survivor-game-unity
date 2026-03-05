using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool playerInRange;
    public GameObject interactPrompt;
    public TMP_Text promptText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        promptText.color = CoinManager.Instance.Balance >= ChestSpawner.Instance.GetNextChestCost()
            ? Color.white: Color.red;
        if (playerInRange && Input.GetKeyDown(KeyCode.E) 
            && CoinManager.Instance.Balance >= (int)ChestSpawner.Instance.GetNextChestCost())
        {
            interactPrompt.SetActive(false);
            OpenChest();
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // prikazi cenu chesta
            interactPrompt.SetActive(true);
            promptText.text = $"[E] Open ({(int)ChestSpawner.Instance.GetNextChestCost()} coins)";
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
            playerInRange = false;
        }
    }

    private void OpenChest(){
        CoinManager.Instance.SpendCoins((int)ChestSpawner.Instance.GetNextChestCost());
        List<UpgradeOption> upgrades = UpgradeManager.Instance.GetThreeUpgrades();
        ChestUIManager.Instance.ShowChest(upgrades);
        ChestSpawner.Instance.ChestOpened();
        Destroy(gameObject, 1f);
    }
}

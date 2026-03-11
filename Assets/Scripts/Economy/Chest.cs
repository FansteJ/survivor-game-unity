using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool playerInRange;
    public GameObject interactPrompt;
    public TMP_Text promptText;
    public Animator animator;

    private bool isOpened = false;
    private static int lastInteractedFrame = -1;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (playerInRange && !isOpened)
        {
            promptText.color = CoinManager.Instance.Balance >= ChestSpawner.Instance.GetNextChestCost()
                ? Color.white : Color.red;

            if (Input.GetKeyDown(KeyCode.E) && CoinManager.Instance.Balance >= (int)ChestSpawner.Instance.GetNextChestCost())
            {
                if (Time.frameCount == lastInteractedFrame)
                    return;

                lastInteractedFrame = Time.frameCount;

                interactPrompt.SetActive(false);
                OpenChest();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
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
        isOpened = true;

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        CoinManager.Instance.SpendCoins((int)ChestSpawner.Instance.GetNextChestCost());
        List<UpgradeOption> upgrades = UpgradeManager.Instance.GetThreeUpgrades();
        ChestUIManager.Instance.ShowChest(upgrades);
        ChestSpawner.Instance.ChestOpened();
        Destroy(gameObject, 2f);
    }
}

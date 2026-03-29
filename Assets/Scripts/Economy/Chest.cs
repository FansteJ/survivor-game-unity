using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool playerInRange;
    public GameObject interactPrompt;
    public TMP_Text promptText;
    public Animator animator;

    private Outline outlineScript;

    private bool isOpened = false;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        outlineScript = GetComponent<Outline>();
        if (outlineScript != null)
        {
            outlineScript.enabled = true;
        }
    }

    void Update()
    {
        if (playerInRange && !isOpened)
        {
            if (ChestUIManager.Instance.IsUIOpen)
            {
                if (interactPrompt.activeSelf) interactPrompt.SetActive(false);
                return;
            }

            if (!interactPrompt.activeSelf) interactPrompt.SetActive(true);

            promptText.color = CoinManager.Instance.Balance >= ChestSpawner.Instance.GetNextChestCost()
                ? Color.white : Color.red;

            if (Input.GetKeyDown(KeyCode.E) && CoinManager.Instance.Balance >= (int)ChestSpawner.Instance.GetNextChestCost())
            {
                interactPrompt.SetActive(false);
                OpenChest();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            if (!ChestUIManager.Instance.IsUIOpen)
            {
                interactPrompt.SetActive(true);
            }
            promptText.text = $"[E] Open ({(int)ChestSpawner.Instance.GetNextChestCost()} coins)";
            playerInRange = true;

            if (outlineScript != null)
            {
                outlineScript.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
            playerInRange = false;

            if (outlineScript != null)
            {
                outlineScript.enabled = true;
            }
        }
    }

    private void OpenChest(){
        isOpened = true;

        if (outlineScript != null)
        {
            outlineScript.enabled = false;
        }

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

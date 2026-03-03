using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool playerInRange;
    public GameObject interactPrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactPrompt.SetActive(false);
            OpenChest();
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
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
        List<UpgradeOption> upgrades = UpgradeManager.Instance.GetThreeUpgrades();
        ChestUIManager.Instance.ShowChest(upgrades);
        Destroy(gameObject, 1f);
    }
}

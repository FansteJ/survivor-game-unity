using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;
    public TMP_Text healthText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = playerHealth.currentHealth / playerHealth.maxHealth * 100;
        healthText.SetText(playerHealth.currentHealth + " / " + playerHealth.maxHealth);
    }
}

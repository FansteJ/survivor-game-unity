using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;
    public TMP_Text healthText;

    void Update()
    {
        float targetValue = playerHealth.currentHealth / playerHealth.maxHealth * 100;
        healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * 5f);
        healthText.SetText(Mathf.RoundToInt(playerHealth.currentHealth) + " / " + Mathf.RoundToInt(playerHealth.maxHealth));
    }
}

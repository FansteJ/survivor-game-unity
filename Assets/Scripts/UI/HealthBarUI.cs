using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;
    public TMP_Text healthText;

    private void Start()
    {
        PlayerHealth.Instance.OnHealthChange += UpdateHealth;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        float targetValue = playerHealth.currentHealth / playerHealth.maxHealth * 100;
        healthText.SetText(Mathf.RoundToInt(playerHealth.currentHealth) + " / " + Mathf.RoundToInt(playerHealth.maxHealth));
        StopAllCoroutines();
        StartCoroutine(AnimateHealth(targetValue));
    }

    IEnumerator AnimateHealth(float target)
    {
        while (Mathf.Abs(healthSlider.value - target) > 0.001f)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, target, Time.deltaTime * 5f);
            yield return null;
        }
        healthSlider.value = target;
    }

    private void OnDestroy()
    {
        PlayerHealth.Instance.OnHealthChange -= UpdateHealth;
    }
}

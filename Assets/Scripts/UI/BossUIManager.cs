using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject bossPanel;
    public Slider hpSlider;
    public TMP_Text hpText;

    private void Awake()
    {
        Instance = this;
        if (bossPanel != null)
        {
            bossPanel.SetActive(false);
        }
    }

    public void ShowBossUI()
    {
        if (bossPanel != null) bossPanel.SetActive(true);
    }

    public void UpdateHP(float currentHp, float maxHp)
    {
        if (hpSlider != null)
        {
            hpText.text = $"{currentHp} / {maxHp}";
            hpSlider.value = currentHp / maxHp;
        }
    }

    public void HideBossUI()
    {
        if (bossPanel != null) bossPanel.SetActive(false);
    }
}
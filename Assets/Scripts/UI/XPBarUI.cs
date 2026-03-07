using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    public Slider XPSlider;
    public TMP_Text XPText;

    void Update()
    {
        float xpNeeded = ExperienceManager.Instance.GetXPForNextLevel();
        float targetValue = ExperienceManager.Instance.CurrentXP / xpNeeded * 100;
        XPSlider.value = Mathf.Lerp(XPSlider.value, targetValue, Time.deltaTime * 5f);
        XPText.SetText(Mathf.RoundToInt(ExperienceManager.Instance.CurrentXP) + " / " + Mathf.RoundToInt(xpNeeded));
    }
}

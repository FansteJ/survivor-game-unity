using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    public Slider XPSlider;
    public TMP_Text XPText;

    private void Start()
    {
        ExperienceManager.Instance.OnXPChanged += UpdateXP;
        UpdateXP();
    }

    void UpdateXP()
    {
        float xpNeeded = ExperienceManager.Instance.GetXPForNextLevel();
        float targetValue = ExperienceManager.Instance.CurrentXP / xpNeeded * 100;
        XPText.SetText(Mathf.RoundToInt(ExperienceManager.Instance.CurrentXP) + " / " + Mathf.RoundToInt(xpNeeded));
        StopAllCoroutines();
        StartCoroutine(AnimateXP(targetValue));
    }

    IEnumerator AnimateXP(float target)
    {
        while (Mathf.Abs(XPSlider.value - target) > 0.001f)
        {
            XPSlider.value = Mathf.Lerp(XPSlider.value, target, Time.deltaTime * 5f);
            yield return null;
        }
        XPSlider.value = target;
    }

    private void OnDestroy()
    {
        ExperienceManager.Instance.OnXPChanged -= UpdateXP;
    }
}

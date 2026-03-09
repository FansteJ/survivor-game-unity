using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{   
    public TMP_Text levelText;
    void Start()
    {
        ExperienceManager.Instance.OnLevelUpEvent += UpdateLevelText;
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        levelText.SetText("Level: " + ExperienceManager.Instance.CurrentLevel);
    }
}

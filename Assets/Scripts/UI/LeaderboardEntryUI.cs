using UnityEngine;
using TMPro;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text usernameText;
    public TMP_Text levelText;

    public void Setup(LeaderboardEntryDTO dto, bool isCurrentUser)
    {
        rankText.text = dto.rank.ToString();
        usernameText.text = dto.username;
        levelText.text = "Lvl " + dto.levelReached.ToString();

        if (isCurrentUser)
        {
            rankText.color = Color.yellow;
            usernameText.color = Color.yellow;
            levelText.color = Color.yellow;
        }
    }
}
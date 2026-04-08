using System;
using UnityEngine;
using TMPro;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text usernameText;
    public TMP_Text durationText;

    public void Setup(LeaderboardEntryDTO dto, bool isCurrentUser)
    {
        rankText.text = dto.rank.ToString();
        usernameText.text = dto.username;

        TimeSpan time = TimeSpan.FromSeconds(dto.duration);

        string timeFormat = time.TotalHours >= 1 ? @"hh\:mm\:ss" : @"mm\:ss";

        durationText.text = time.ToString(timeFormat);

        if (isCurrentUser)
        {
            rankText.color = Color.yellow;
            usernameText.color = Color.yellow;
            durationText.color = Color.yellow;
        }
    }
}
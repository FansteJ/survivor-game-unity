using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetLeaderboard(Action<List<LeaderboardEntryDTO>> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetLeaderboardCoroutine(onSuccess, onError));
    }

    private IEnumerator GetLeaderboardCoroutine(Action<List<LeaderboardEntryDTO>> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + "/api/leaderboard?page=0&size=10";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var entries = JsonConvert.DeserializeObject<List<LeaderboardEntryDTO>>(webRequest.downloadHandler.text);
                onSuccess(entries);
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }
}
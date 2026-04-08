using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public PlayerModifiers activeModifiers { get; set; }
    void Awake()
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

    public void GetShopItems(Action<List<UpgradeShopItemDTO>> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetShopItemsCoroutine(onSuccess, onError));
    }

    private IEnumerator GetShopItemsCoroutine(Action<List<UpgradeShopItemDTO>> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + "/api/player/upgrades";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var items = JsonConvert.DeserializeObject<List<UpgradeShopItemDTO>>(webRequest.downloadHandler.text);
                onSuccess(items);
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }

    public void BuyUpgrade(string upgradeId, Action<BuyUpgradeResponse> onSuccess, Action<string> onError)
    {
        StartCoroutine(BuyUpgradeCoroutine(upgradeId, onSuccess, onError));
    }

    private IEnumerator BuyUpgradeCoroutine(string upgradeId, Action<BuyUpgradeResponse> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + $"/api/player/upgrades/{upgradeId}/buy";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                BuyUpgradeResponse response = JsonConvert.DeserializeObject<BuyUpgradeResponse>(webRequest.downloadHandler.text);

                if (ApiManager.Instance.CurrentProfile != null)
                {
                    ApiManager.Instance.CurrentProfile.gold = response.remainingGold;
                }

                onSuccess(response);
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }

    public void GetPlayerModifiers(Action<PlayerModifiers> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetPlayerModifiersCoroutine(onSuccess, onError));
    }

    private IEnumerator GetPlayerModifiersCoroutine(Action<PlayerModifiers> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + "/api/player/modifiers";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                PlayerModifiers modifiers = JsonConvert.DeserializeObject<PlayerModifiers>(webRequest.downloadHandler.text);
                activeModifiers = modifiers;
                onSuccess(modifiers);
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }

    public void GetDailyQuests(Action<List<UserQuestDTO>> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetDailyQuestsCoroutine(onSuccess, onError));
    }

    private IEnumerator GetDailyQuestsCoroutine(Action<List<UserQuestDTO>> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + "/api/quests";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var quests = JsonConvert.DeserializeObject<List<UserQuestDTO>>(webRequest.downloadHandler.text);
                onSuccess(quests);
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }

    public void ClaimQuestReward(string questId, Action<string> onSuccess, Action<string> onError)
    {
        StartCoroutine(ClaimQuestRewardCoroutine(questId, onSuccess, onError));
    }

    private IEnumerator ClaimQuestRewardCoroutine(string questId, Action<string> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + $"/api/quests/{questId}/claim";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ClaimRewardResponse response = JsonConvert.DeserializeObject<ClaimRewardResponse>(webRequest.downloadHandler.text);

                if (ApiManager.Instance.CurrentProfile != null)
                {
                    ApiManager.Instance.CurrentProfile.gems = response.newGemBalance;
                }

                onSuccess("Claimed!");
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }
}
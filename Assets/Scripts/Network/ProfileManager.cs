using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetProfile(Action<UserProfileDTO> onSuccess, Action<string> onError)
    {
        StartCoroutine(GetProfileCoroutine(onSuccess, onError));
    }

    IEnumerator GetProfileCoroutine(Action<UserProfileDTO> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + "/api/profile/me";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                UserProfileDTO dto = JsonUtility.FromJson<UserProfileDTO>(webRequest.downloadHandler.text);
                onSuccess(dto);
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }
}

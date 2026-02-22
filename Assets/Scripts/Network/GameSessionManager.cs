using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance { get; private set; }

    public string CurrentSessionId { get; private set; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame(Action<string> onSuccess, Action<string> onError)
    {
        StartCoroutine(StartGameCoroutine(onSuccess, onError));
    }

    IEnumerator StartGameCoroutine(Action<string> onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + "/api/game/start";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                CurrentSessionId = request.downloadHandler.text;
                onSuccess(request.downloadHandler.text);
            }
            else
            {
                onError(request.downloadHandler.text);
            }
        }
    }

    public void FinishGame(FinishGameSessionRequest finishRequest, Action onSuccess, Action<string> onError)
    {
        StartCoroutine(FinishGameCoroutine(finishRequest, onSuccess, onError));
    }

    IEnumerator FinishGameCoroutine(FinishGameSessionRequest finishRequest, Action onSuccess, Action<string> onError)
    {
        string token = ApiManager.Instance.GetToken();
        string url = ApiManager.Instance.baseUrl + "/api/game/finish";

        string json = JsonUtility.ToJson(finishRequest);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess();
            }
            else
            {
                onError(request.downloadHandler.text);
            }
        }
    }
}

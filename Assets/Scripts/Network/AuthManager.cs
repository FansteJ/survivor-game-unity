using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }

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

    public void Login(string username, string password, Action onSuccess, Action<string> onError)
    {
        StartCoroutine(LoginCoroutine(username, password, onSuccess, onError));
    }

    IEnumerator LoginCoroutine(string username, string password, Action onSuccess, Action<string> onError)
    {
        LoginRequest request = new LoginRequest();
        request.username = username;
        request.password = password;

        string json = JsonUtility.ToJson(request);
        string url = ApiManager.Instance.baseUrl + "/api/auth/login";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                AuthResponse response = JsonUtility.FromJson<AuthResponse>(webRequest.downloadHandler.text);
                ApiManager.Instance.SetToken(response.token);
                onSuccess();
            }
            else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }

    public void Register(string username, string password, Action onSuccess, Action<string> onError)
    {
        StartCoroutine(RegisterCoroutine(username, password, onSuccess, onError));
    }

    IEnumerator RegisterCoroutine(string username, string password, Action onSuccess, Action<string> onError)
    {
        RegisterRequest request = new RegisterRequest();
        request.username = username;
        request.password = password;

        string json = JsonUtility.ToJson(request);
        string url = ApiManager.Instance.baseUrl + "/api/auth/register";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess();
            } else
            {
                onError(webRequest.downloadHandler.text);
            }
        }
    }
}

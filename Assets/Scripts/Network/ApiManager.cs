using UnityEngine;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance { get; private set; }

    public string baseUrl = "http://localhost:8080";
    private string token {  get; set; }

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

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(token);
    }

    public void SetToken(string token)
    {
        this.token = token;
    }
}
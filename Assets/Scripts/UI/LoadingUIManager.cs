using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingUIManager : MonoBehaviour
{
    void Start()
    {
        ProfileManager.Instance.GetProfile(OnSuccess, OnError);
    }

    private void OnSuccess(UserProfileDTO dto)
    {
        ApiManager.Instance.SetProfile(dto);
        SceneManager.LoadScene("MainMenu");
    }

    private void OnError(string error)
    {
        SceneManager.LoadScene("Login");
    }
}

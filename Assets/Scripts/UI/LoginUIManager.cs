using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public TMP_Text errorText;

    void Start()
    {
        loginButton.onClick.AddListener(SendLogin);
        registerButton.onClick.AddListener(RegisterLogin);
    }

    private void SendLogin()
    {
        AuthManager.Instance.Login(usernameInput.text, passwordInput.text, OnSuccess, OnError);
    }
    
    private void RegisterLogin()
    {
        AuthManager.Instance.Register(usernameInput.text, passwordInput.text, OnSuccessRegister, OnError);
    }

    private void OnSuccess()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnSuccessRegister()
    {
        // TODO
    }

    private void OnError(string error)
    {
        if (string.IsNullOrEmpty(error))
            errorText.SetText("Invalid username or password");
        else
            errorText.SetText(error);
    }
}

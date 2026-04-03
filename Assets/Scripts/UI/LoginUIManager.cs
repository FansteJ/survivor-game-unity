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

        usernameInput.onSubmit.AddListener(OnUsernameEnter);
        passwordInput.onSubmit.AddListener(OnPasswordEnter);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (usernameInput.isFocused)
            {
                passwordInput.Select();
                passwordInput.ActivateInputField();
            }
            else if (passwordInput.isFocused)
            {
                usernameInput.Select();
                usernameInput.ActivateInputField();
            }
        }
    }

    private void OnUsernameEnter(string text)
    {
        passwordInput.Select();
        passwordInput.ActivateInputField();
    }

    private void OnPasswordEnter(string text)
    {
        SendLogin();
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
        SceneManager.LoadScene("Loading");
    }

    private void OnSuccessRegister()
    {
        errorText.color = Color.green;
        errorText.SetText("Register successful!");
    }

    private void OnError(string error)
    {
        errorText.color = Color.red;
        if (string.IsNullOrEmpty(error))
            errorText.SetText("Invalid username or password");
        else
            errorText.SetText(error);
    }
}
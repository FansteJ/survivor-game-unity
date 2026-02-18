[System.Serializable]
public class LoginRequest
{
    public string username;
    public string password;
}

[System.Serializable]
public class RegisterRequest
{
    public string username;
    public string password;
}

[System.Serializable]
public class AuthResponse
{
    public string token;
}

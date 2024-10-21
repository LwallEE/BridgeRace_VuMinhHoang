using System;

[Serializable]
public class LoginRequest
{
    public string userAccount;
    public string userPassword;

    public LoginRequest(string account, string userPassword)
    {
        this.userAccount = account;
        this.userPassword = userPassword;
    }
}

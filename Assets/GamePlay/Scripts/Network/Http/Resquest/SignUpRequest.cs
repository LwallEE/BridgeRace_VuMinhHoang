using System;

[Serializable]
public class SignUpRequest
{
    public string userAccount;
    public string userPassword;
    public string userName;

    public SignUpRequest(string account, string userPassword, string name)
    {
        this.userAccount = account;
        this.userPassword = userPassword;
        this.userName = name;
    }
}

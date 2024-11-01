public class LoginResponse : GeneralResponse
{
    public string token;

    public LoginResponse(bool isSuccess, string message) : base(isSuccess, message)
    {
    }

    public LoginResponse() : base()
    {}

}
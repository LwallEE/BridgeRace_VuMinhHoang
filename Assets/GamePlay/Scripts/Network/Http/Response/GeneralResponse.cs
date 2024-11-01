
using System;

[Serializable]
public class GeneralResponse
{
    public bool isSuccess;
    
    public string message;

    public GeneralResponse(bool isSuccess, string message)
    {
        this.isSuccess = isSuccess;
        this.message = message;
    }
}
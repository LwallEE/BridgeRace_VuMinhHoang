using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameDevWare.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class TestAPI : MonoBehaviour
{
    public string accountName;
    public string passWord;

    public string token;

    async void Start()
    {
       
    }

    [ContextMenu("loginAPI")]
    public async Task TestLoginApi()
    {
        Debug.Log("start sending..");
        var result = await NetworkClient.Instance.HttpPost<LoginResponse>("login", new LoginRequest(accountName, passWord));
        Debug.Log(result.token+ " "+result.isSuccess + " " + result.message);
    }

    [ContextMenu("TestToken")]
    public async Task TestToken()
    {
        NetworkClient.Instance.SetToken(token);
        var result = await NetworkClient.Instance.HttpGet<GeneralResponse>("login");
        Debug.Log(result.isSuccess + " " + result.message);
    }
   
    

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Colyseus;
using MyGame.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkClient : MonoBehaviour
{
    public string roomType = "game_room";
    public string hostName = "localhost";
    public string portName = "2567";

    [SerializeField] private float timeToCallGetPingFromServer=2f;
    [SerializeField] private float timeToCheckDisconnect;
    private ColyseusClient client;
    
    private ColyseusRoom<GameRoomState> gameRoom;

    private float startTime;
    private bool isReceivePing;
    private ReconnectionToken roomReconnectionToken;
    public bool IsConnect { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }
    

    async void Connect()
    {
        string endpoint = "ws://" + hostName + ":" + portName;
        client = new ColyseusClient(endpoint);
        await ConnectToGameRoom();
        GameNetworkManager.Instance.OnInit(this);
    }

    async Task ConnectToGameRoom()
    {
        gameRoom = await client.JoinOrCreate<GameRoomState>(roomType);
        InitRoom();
    }

    void InitRoom()
    {
        roomReconnectionToken = gameRoom.ReconnectionToken;
        RegisterEventFromServer();
        IsConnect = true;
        StartCoroutine(GetPingFromServer());
        
    }

    void RegisterEventFromServer()
    {
        gameRoom.OnJoin += GameRoomOnJoin;
        gameRoom.OnError += GameRoomOnError;
        gameRoom.OnLeave += GameRoomOnLeave;
        
        gameRoom.OnMessage<string>("getPing", s =>
        {
            float endTime = Time.time;
            isReceivePing = true;
            //IsConnect = true;
            Debug.Log("Ping " + (endTime-startTime)*1000 + " ms");
        } );
        gameRoom.OnMessage<string>("check-collide-result",message =>
        {
            Debug.Log(message);
        });
        gameRoom.State.players.OnAdd(OnAddPlayer);
        gameRoom.State.map.OnChange(OnMapChange);
    }

    private void OnMapChange()
    {
        Debug.Log("map change");
       
       //To Do: Initialize map here
       GameNetworkManager.Instance.GenerateMap(gameRoom.State.map);
    }


    #region CallBackFromServer
    private void OnAddPlayer(string key, PlayerData player)
    {
        if (key == gameRoom.SessionId)
        {
         
            GameNetworkManager.Instance.SetMainPlayer(player);
        }
        else
        {
            GameNetworkManager.Instance.AddOtherPlayer(player);

            
        }
    }

    


    private async void GameRoomOnLeave(int code)
    {
        
        //throw new System.NotImplementedException();
        Debug.Log("has leave " + code);
        if (code >= 4000) return;
        RoomDispose();
        await Reconnect();
        
    }
    
    //dispose room and client here
    private void RoomDispose()
    {
        Debug.Log("dispose");
        //gameRoom = null;
        GameNetworkManager.Instance.Dispose();
        StopAllCoroutines();
    }

    private void GameRoomOnError(int code, string message)
    {
        Debug.LogError(message);
    }

    private void GameRoomOnJoin()
    {
        Debug.Log("has join game");
    }
    private void Disconnect()
    {
        if (IsConnect)
        {
            Debug.Log("disconnect");
            IsConnect = false;
            gameRoom.Leave(false);
            //deal with disconnect here
        }
    }

    private async Task<bool> Reconnect()
    {
        try
        {
            gameRoom = await client.Reconnect<GameRoomState>(roomReconnectionToken);
            Debug.Log("joined successfully");
            InitRoom();
            //deal with reconnect here
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
            
        }
    }

    public bool CheckRoomAvailable()
    {
        return gameRoom != null;
    }
        #endregion
        
    #region ActionFromClient
    IEnumerator GetPingFromServer()
    {
        WaitForSeconds wait = new WaitForSeconds(timeToCallGetPingFromServer);
        WaitForSeconds checkDisconectTime = new WaitForSeconds(timeToCheckDisconnect);
        while (true)
        {
            GetPing();
            yield return checkDisconectTime;
            if (!isReceivePing)
            {
                Disconnect();
            }
            yield return wait;
        }
    }

   
    async void GetPing()
    {
        startTime = Time.time;
        isReceivePing = false;
        SendMessageToServer("ping",null);
       
    }

   

    public void SendMessageToServer(string type, object message)
    {
        if(gameRoom != null && IsConnect)
            gameRoom.Send(type, message);
    }

    public void SendMessageToServer(string type, object message, int attempt,float timeAttempt)
    {
        if (gameRoom != null)
        {
            StartCoroutine(AttempSend(type, message, attempt,timeAttempt));
        }
    }

    public void SendMessageToServer(string type, object message, float delay)
    {
        if (gameRoom != null)
        {
            StartCoroutine(DelaySend(type, message, delay));
        }
    }
    IEnumerator AttempSend(string type, object message,int attempt,float timeAttemptSend)
    {
        
        WaitForSeconds wait = new WaitForSeconds(timeAttemptSend);
        for (int i = 0; i < attempt; i++)
        {
            
            SendMessageToServer(type, message);
            yield return wait;
        }
    }

    IEnumerator DelaySend(string type, object message, float delay)
    {
        yield return new WaitForSeconds(delay);
        SendMessageToServer(type,message);
    }
    
    #endregion

    #region Other

    public PlayerData GetPlayerData(string key)
    {
        PlayerData result;
        if (gameRoom.State.players.TryGetValue(key, out result))
        {
            return result;
        }

        return null;
    }
    

    #endregion
    

    void OnApplicationQuit()
    {
        if(gameRoom != null)
            gameRoom.Leave(true);
    }
}

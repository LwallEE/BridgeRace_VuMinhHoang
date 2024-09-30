using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using MyGame.Schema;
using UnityEngine;

public class GameRoomNetwork : MonoBehaviour
{
    [SerializeField] private float timeToCallGetPingFromServer = 2f;
    [SerializeField] private float timeToCheckDisconnect;
    private ColyseusRoom<GameRoomState> gameRoom;

    private float startTime;
    private bool isReceivePing;
    private ReconnectionToken roomReconnectionToken;
    private ColyseusClient client;
    private string roomType;
    public bool IsConnect { get; private set; }

    public void InitGameRoom(ColyseusClient client, string roomName)
    {
        this.client = client;
        this.roomType = roomName;
    }

    public async Task CreateGameRoom(string userName, string roomName)
    {
        gameRoom = await client.Create<GameRoomState>(roomType, new Dictionary<string, object>()
        {
            ["name"] = roomName,
            ["userName"] = userName,
            ["score"] = PlayerSaveData.CurrentAchievement
        });
        InitRoom();
    }
    public async Task JoinGameRoom(string userName, string roomId)
    {
        gameRoom = await client.JoinById<GameRoomState>(roomId, new Dictionary<string, object>()
        {
            ["userName"] = userName,
            ["score"] = PlayerSaveData.CurrentAchievement
        });
        InitRoom();
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
            GameNetworkManager.Instance.PingChange((endTime-startTime)*1000);
            //Debug.Log("Ping " + (endTime-startTime)*1000 + " ms");
        } );
        gameRoom.OnMessage<string>("check-collide-result",message =>
        {
            Debug.Log(message);
        });
        gameRoom.OnMessage<Vect3>("fix-position", message =>
        {
            Debug.Log("fix player position");
            GameNetworkManager.Instance.FixPlayerPosition(message);
        });
        gameRoom.OnMessage<Vect3>("fall-player", message =>
        {
            Debug.Log("player fall " + NetworkUltilityHelper.ConvertFromVect3ToVector3(message));
            GameNetworkManager.Instance.PlayerFall(message);
        });
        gameRoom.OnMessage<ResultGameResponse>("game-result", message =>
        {
            GameNetworkManager.Instance.GameResult(message);
        });
        gameRoom.State.OnGameStateChange((value, previousValue) =>
        {
            GameNetworkManager.Instance.SetGameState((GameNetworkStateEnum)value);
        });
        gameRoom.State.players.OnAdd(OnAddPlayer);
        gameRoom.State.map.OnChange(OnMapChange);
        gameRoom.State.OnCountDownTimeChange((value, previousValue) =>
        {
            GameNetworkManager.Instance.CountDownTextChange((int)value);
        });
        gameRoom.State.networkUsers.OnAdd((key, value) =>
        {
            GameNetworkManager.Instance.PlayerJoinRoomWaiting(value);
        });
        gameRoom.State.networkUsers.OnRemove((key, value) =>
        {
            GameNetworkManager.Instance.PlayerLeaveRoomWaiting(key);
        });
    }
    void InitRoom()
    {
        roomReconnectionToken = gameRoom.ReconnectionToken;
        RegisterEventFromServer();
        IsConnect = true;
        StartCoroutine(GetPingFromServer());
        
    }

    public bool IsGameRoomConnect()
    {
        return gameRoom != null;
    }
    #region CallBackFromServer
    private void OnMapChange()
    {
        Debug.Log("map change");
        GameNetworkManager.Instance.GenerateMap(gameRoom.State.map);
    }
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
        UINetworkManager.Instance.OpenNetworkErrorPanel();
        
    }
    
    //dispose room and client here
    private void RoomDispose()
    {
        //gameRoom = null;
        GameNetworkManager.Instance.Dispose();
        StopAllCoroutines();
    }

    private void GameRoomOnError(int code, string message)
    {
        Debug.Log("error");
        Debug.Log(message);
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
            UINetworkManager.Instance.OpenNetworkErrorPanel();
            IsConnect = false;
            //deal with disconnect here
        }
    }

    public async Task<bool> Reconnect()
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
            UINetworkManager.Instance.OpenPopUpMessage(e.Message, () =>
            {
                UINetworkManager.Instance.GetPopUpMessageUI().Close();
            });
            return false;
            
        }
    }

    public string GetSessionId()
    {
        if (gameRoom == null) return "";
        return gameRoom.SessionId;
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

        public async Task LeaveGameRoom()
        {
            await gameRoom.Leave(true);
            RoomDispose();
            gameRoom = null;
        }
    
        #endregion
        
    #region Other

    public NetworkUserData GetNetworkUserData(string key)
    {
        NetworkUserData result;
        if (gameRoom.State.networkUsers.TryGetValue(key, out result))
        {
            return result;
        }

        return null;
    }

    public int GetNumberOfUser()
    {
        if (gameRoom == null) return 0;
        return gameRoom.State.networkUsers.Count;
    }
    private PlayerData GetPlayerData(string key)
    {
        PlayerData result;
        if (gameRoom.State.players.TryGetValue(key, out result))
        {
            return result;
        }

        return null;
    }

    private BrickData GetGreyBrickData(string key)
    {
        BrickData result;
        if (gameRoom.State.map.greyBricks.TryGetValue(key, out result))
        {
            return result;
        }

        return null;
    }
    public bool IsChangePosition(string key, Vector3 position)
    {
        Vect3 previousPlayerPosition = GetEntityData(key).position;
        return NetworkUltilityHelper.ConvertFromVect3ToVector3(previousPlayerPosition) != position;
    }

    private EntityData GetEntityData(string key)
    {
        EntityData result = null;
        result = GetPlayerData(key);
        if (result != null) return result;
        result = GetGreyBrickData(key);
        if (result != null) return result;
        return null;
    }

    public PlayerData GetWinPlayerData()
    {
        if (gameRoom == null) return null;
        return gameRoom.State.winPlayer;
    }

    public int GetMinNumberOfPlayerToStart()
    {
        if (gameRoom == null) return 0;
        return gameRoom.State.minPlayerToStart;
    }

    public async Task<bool> CheckRoomExist(string roomName)
    {
        if (client == null) return false;
        var rooms = await client.GetAvailableRooms<RoomListingData>(roomType);
        foreach (var room in rooms)
        {
            if (room.metadata.name == roomName)
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}

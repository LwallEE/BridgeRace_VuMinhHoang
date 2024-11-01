using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Colyseus;
using MyGame.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkClient : Singleton<NetworkClient>
{
    public string roomType = "game_room";
    public string lobbyRoomName = "lobby";
    public string hostName = "26.53.215.253";
    public string portName = "2567";
    public bool isHttps;

    public bool isTest;
    [field: SerializeField] public GameRoomNetwork GameRoomNetwork { get; private set; }
    private ColyseusClient client;
    private ColyseusLobby lobbyRoom;
    public string HttpEndpoint => (isHttps ? "https://" : "http://") + hostName + ":" + portName;

    public bool IsConnectToServer { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }
    

    async void Connect()
    {
        try
        {
            string endpoint = "ws://" + hostName + ":" + portName;
            if (isHttps)
            {
                endpoint = "wss://" + hostName + ":" + portName;
            }
            /*if(isHttps)
            {
                endpoint = "wss://" + hostName + ":" + portName;
            }*/
            client = new ColyseusClient(endpoint);

            IsConnectToServer = true;
        }
        catch (Exception e)
        {
            IsConnectToServer = false;
            UINetworkManager.Instance.OpenPopUpMessage(e.Message, () =>
            {
                //<To Do> return the main scene if connection failed
                Debug.Log("back to main menu");
                GameNetworkManager.Instance.BackToMainMenuScene();
            });
        }
      
       
    }

    public async Task<T> HttpGet<T>(string uriPath,string token = "") where T: GeneralResponse
    {
        if (client == null) return default(T);
        client.Http.AuthToken = token;

        try
        {
            return await client.Http.Get<T>(uriPath);
        }
        catch (Exception e)
        {
            return (T)Activator.CreateInstance(typeof(T), false, e.Message);;
        }
    }
    public async Task<T> HttpPost<T>(string uriPath,object data, string token = "") where T: GeneralResponse
    {
      
        if (client == null) return default;
        Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        Type dataType = data.GetType();

        // Get all public fields
        FieldInfo[] fields = dataType.GetFields(BindingFlags.Public | BindingFlags.Instance);

        // Loop through each field and print its name and value
        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(data);
            dataDictionary.Add(field.Name, value);
        }

        client.Http.AuthToken = token;
        try
        {
            return await client.Http.Post<T>(uriPath, dataDictionary);
        }
        catch (Exception e)
        {
            return (T)Activator.CreateInstance(typeof(T), false, e.Message);;
        }
    }
    public async Task ConnectToLobbyRoom()
    {
        try
        {
            if (GameRoomNetwork.IsGameRoomConnect())
            {
                await GameRoomNetwork.LeaveGameRoom();
            }

            lobbyRoom = new ColyseusLobby(client, lobbyRoomName);
            Debug.Log("try to connect lobby");
            await lobbyRoom.Connect();
            Debug.Log("connect to lobby success");
            UINetworkManager.Instance.OpenLobbyRoomPanel();
            lobbyRoom.OnRooms += (rooms) => { UINetworkManager.Instance.GetLobbyPanelUI().InitializeListRoom(rooms); };

            lobbyRoom.OnAddRoom += (roomId, roomInfo) =>
            {
                UINetworkManager.Instance.GetLobbyPanelUI().AddOrUpdateRoom(roomInfo);
            };

            lobbyRoom.OnRemoveRoom += (roomId) => { UINetworkManager.Instance.GetLobbyPanelUI().RemoveRoom(roomId); };
        }
        catch (Exception e)
        {
            UINetworkManager.Instance.OpenPopUpMessage(e.Message, (() =>
            {
                GameNetworkManager.Instance.BackToMainMenuScene();
            }));
        }
    }

    public void LeaveLobbyRoom()
    {
        if (lobbyRoom != null)
        {
            lobbyRoom.LeaveRoom();
        }
    }
    public async Task CreateGameRoom(string userName,string roomName)
    {
        try
        {
            if (lobbyRoom != null)
            {
                await lobbyRoom.LeaveRoom();
            }
            
            /*
            gameRoom = await client.Create<GameRoomState>(roomType, new Dictionary<string, object>()
            {
                ["name"] = roomName,
                ["userName"] = userName
            });
            InitRoom();*/
            GameRoomNetwork.InitGameRoom(this.client, this.roomType);
            await GameRoomNetwork.CreateGameRoom(userName, roomName);
        }
        catch (Exception e)
        {
            UINetworkManager.Instance.OpenPopUpMessage(e.Message, (() =>
            {
                UINetworkManager.Instance.GetPopUpMessageUI().Close();
            }));
        }
    }

    public async Task JoinGameRoom(string userName, string roomId)
    {
        try
        {
            if (lobbyRoom != null)
            {
                await lobbyRoom.LeaveRoom();
            }

            /*
            gameRoom = await client.JoinById<GameRoomState>(roomId, new Dictionary<string, object>()
            {
                ["userName"] = userName,
            });
            InitRoom();
            */
            GameRoomNetwork.InitGameRoom(this.client, this.roomType);
            await GameRoomNetwork.JoinGameRoom(userName, roomId);
        }
        catch (Exception e)
        {
            UINetworkManager.Instance.OpenPopUpMessage(e.Message, (() =>
            {
                UINetworkManager.Instance.GetPopUpMessageUI().Close();
            }));
        }
    }
    void OnApplicationQuit()
    {
        if(GameRoomNetwork.IsGameRoomConnect())
            GameRoomNetwork.LeaveGameRoom();
    }

    internal async Task HttpPost<T>(string v, LoginRequest loginRequest, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}

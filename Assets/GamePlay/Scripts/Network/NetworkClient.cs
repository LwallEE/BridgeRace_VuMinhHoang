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
    private ColyseusClient client;

    private ColyseusRoom<GameRoomState> gameRoom;

    private float startTime;
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
        RegisterEventFromServer();
        //StartCoroutine(GetPingFromServer());
    }

    void RegisterEventFromServer()
    {
        gameRoom.OnJoin += GameRoomOnJoin;
        gameRoom.OnError += GameRoomOnError;
        gameRoom.OnLeave += GameRoomOnLeave;
        
        gameRoom.OnMessage<string>("getPing", s =>
        {
            float endTime = Time.time;
            Debug.Log("Ping " + (endTime-startTime)*1000 + " ms");
        } );
        gameRoom.State.players.OnAdd(OnAddPlayer);
        
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

            player.OnPositionChange(delegate(Vect3 value, Vect3 previousValue)
            {
//                Debug.Log("position change " + NetworkUltilityHelper.ConvertFromVect3ToVector3(value));
                GameNetworkManager.Instance.UpdatePositionOfPlayer(player.entityId, value);
            });
            player.OnYRotationChange((value, previousValue) =>
            {
                GameNetworkManager.Instance.UpdateRotationOfPlayer(player.entityId, value);
            });
            player.OnAnimNameChange((value, previousValue) =>
            {
                //Debug.Log("animName change " + value + " "+previousValue);
                GameNetworkManager.Instance.UpdateAnimNameOfPlayer(player.entityId, value,previousValue);
            });
        }
    }

    


    private void GameRoomOnLeave(int code)
    {
        //throw new System.NotImplementedException();
        Debug.Log("has leave");
        //gameRoom = null;
        //StopAllCoroutines();
    }

    private void GameRoomOnError(int code, string message)
    {
        Debug.LogError(message);
    }

    private void GameRoomOnJoin()
    {
        Debug.Log("has join game");
    }

        #endregion
        
    #region ActionFromClient
    IEnumerator GetPingFromServer()
    {
        WaitForSeconds wait = new WaitForSeconds(timeToCallGetPingFromServer);
        while (true)
        {
            GetPing();
            yield return wait;
        }
    }
   
    async void GetPing()
    {
        startTime = Time.time;
        await gameRoom.Send("ping");
    }

    private async void LeaveRoom()
    {
        await gameRoom.Send("leave-room");
    }

    public void SendMessage<T>(string type, T message)
    {
        gameRoom.Send(type, message);
    }

    public void SendUpdatePosition(PlayerInputMessage message)
    {
        gameRoom.Send("input", message);

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
        Debug.Log("quit game");
        LeaveRoom();
    }
}

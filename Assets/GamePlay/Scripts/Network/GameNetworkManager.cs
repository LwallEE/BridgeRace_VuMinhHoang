using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class GameNetworkManager : Singleton<GameNetworkManager>
{
    private NetworkClient client;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private MapNetwork mapGenerate;
    [field:SerializeField] public float LerpPlayerSpeed { get; private set; }

    [SerializeField] private float timeAttempSend = 0.15f;

    [SerializeField] private float multiplierDelaySend = 1f;
    // Start is called before the first frame update
    private PlayerNetworkController player;

    private Dictionary<string,PlayerNetworkController> otherPlayers;

    private readonly float sendInterval = 100 / 1000f;
    private float sendTimer = 0f;
    private List<IDispose> elementToDispose = new List<IDispose>();

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        sendTimer += Time.deltaTime;
        if (sendTimer >= sendInterval)
        {
            sendTimer = 0f;
            UpdatePositionRotationOfPlayerToServer();
        }
    }

    public void OnInit(NetworkClient client)
    {
        this.client = client;
        gameObject.SetActive(true);
    }

    public void AddToDisposeList(IDispose item)
    {
        this.elementToDispose.Add(item);
    }
    public void SetMainPlayer(PlayerData data)
    {
        this.player = Instantiate(playerPrefab).GetComponent<PlayerNetworkController>();
        player.InitPlayerNetwork(data, true);
        cameraFollow.SetTarget(player.transform);
    }

    public void AddOtherPlayer(PlayerData data)
    {
        if (otherPlayers == null)
        {
            otherPlayers = new Dictionary<string, PlayerNetworkController>();
        }

        var otherPlayer = Instantiate(playerPrefab).GetComponent<PlayerNetworkController>();
        otherPlayer.InitPlayerNetwork(data, false);
        otherPlayers.TryAdd(data.entityId, otherPlayer);
    }

    private PlayerNetworkController GetPlayer(string key)
    {
        PlayerNetworkController playerr ;
        if (otherPlayers.TryGetValue(key, out playerr))
        {
            return playerr;
        }
        return null;
    }

    public void GenerateMap(MapData data)
    {
        mapGenerate.InitMap(data);
    }

    public void Dispose()
    {
        foreach (var item in elementToDispose)
        {
            item.Dispose();
        }
        elementToDispose.Clear();
        mapGenerate.Dispose();
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }

        if (otherPlayers != null)
        {
            foreach (var item in otherPlayers)
            {
                Destroy(item.Value.gameObject);
            }
            otherPlayers.Clear();
        }
       
    }
    
    

    #region MessageSendToServer
    public void UpdatePositionRotationOfPlayerToServer()
    {
        if (player == null || client == null || !client.IsConnect || !client.CheckRoomAvailable()) return;
        //Compare the last player data's position and current's position
        //if not change return
       
        Vect3 previousPlayerPosition = client.GetPlayerData(player.Id).position;
        bool check = NetworkUltilityHelper.ConvertFromVect3ToVector3(previousPlayerPosition) !=
                     player.transform.position;
        if (!check) return;
        
        PlayerInputMessage message = new PlayerInputMessage()
        {
            position = NetworkUltilityHelper.ConvertFromVector3ToVect3(player.transform.position),
            yRotation = player.transform.eulerAngles.y
        };
       
       
        
           
            client.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_PLAYER_POSITION_ROTATION,message);
        
    }

    public void UpdateAnimationOfPlayerToServer(string newState)
    {
        if (player == null || client == null || !client.IsConnect) return;
        client.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_PLAYER_ANIMATION, newState);
    }

    public void RequestCollectBrick(string brickId)
    {
        if (player == null || client == null || !client.IsConnect) return;
        //client.SendMessage<string>(CommandFromClient.COMMAND_PLAYER_COLLECT_BRICK, brickId);
        Debug.Log("colelct brick");
        client.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_COLLECT_BRICK, brickId, sendInterval*multiplierDelaySend);
    }

   
    

    #endregion
   
}

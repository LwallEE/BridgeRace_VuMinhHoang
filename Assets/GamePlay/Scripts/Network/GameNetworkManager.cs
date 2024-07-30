using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class GameNetworkManager : Singleton<GameNetworkManager>
{
    public NetworkClient Client { get; private set; }
    
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
    private bool isFreezingSending;
    private GameNetworkStateEnum gameState;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsInGameState(GameNetworkStateEnum.GameLoop)) return;
        sendTimer += Time.deltaTime;
        if (sendTimer >= sendInterval)
        {
            sendTimer = 0f;
            UpdatePositionRotationOfPlayerToServer();
            UpdateGreyBrickPosition();
        }
    }

    public void OnInit(NetworkClient client)
    {
        this.Client = client;
        gameObject.SetActive(true);
    }

    public void AddToDisposeList(IDispose item)
    {
        this.elementToDispose.Add(item);
    }

    #region CallBackFromServer
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

    public void FixPlayerPosition(Vect3 position)
    {
        if (player != null)
        {
            player.transform.position = NetworkUltilityHelper.ConvertFromVect3ToVector3(position);
        }
    }

    public void PlayerFall(Vect3 direction)
    {
        if (player != null)
        {
            player.Fall(NetworkUltilityHelper.ConvertFromVect3ToVector3(direction));
        }
    }

    public void SetGameState(GameNetworkStateEnum state)
    {
        if (state == this.gameState) return;
        this.gameState = state;
        Debug.Log("Change game state " + state);
        if (this.gameState == GameNetworkStateEnum.GameEnd)
        {
            //Show win panel here
            var winPlayer = Client.GetWinPlayerData();
            if (winPlayer == null) return;
            Debug.Log("game end " + winPlayer.entityId);
        }
    }
    

    #endregion
    
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

    public bool IsInGameState(GameNetworkStateEnum state)
    {
        return this.gameState == state;
    }

    #region MessageSendToServer
    public void UpdatePositionRotationOfPlayerToServer()
    {
        if (player == null || Client == null || !Client.IsConnect || !Client.CheckRoomAvailable()) return;
        //Compare the last player data's position and current's position
        //if not change return
        
        if (!Client.IsChangePosition(player.Id, player.transform.position)) return;
        
        PlayerInputMessage message = new PlayerInputMessage()
        {
            position = NetworkUltilityHelper.ConvertFromVector3ToVect3(player.transform.position),
            yRotation = player.transform.eulerAngles.y
        };
       
       
        
           
        Client.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_PLAYER_POSITION_ROTATION,message);
        
    }

    public void UpdateGreyBrickPosition()
    {
        if (mapGenerate == null || Client == null || !Client.IsConnect) return;
        var message = mapGenerate.GetGreyBrickMessage();
        if (message.brickChanges == null || message.brickChanges.Count <= 0) return;
        Debug.Log("update grey brick");
        Client.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_GREY_BRICK, message);
    }

    public void UpdateAnimationOfPlayerToServer(string newState)
    {
        if (player == null || Client == null || !Client.IsConnect) return;
        Client.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_PLAYER_ANIMATION, newState);
    }

    public void RequestCollectBrick(string brickId)
    {
        if (player == null || Client == null || !Client.IsConnect) return;
        Client.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_COLLECT_BRICK, brickId, sendInterval*multiplierDelaySend);
    }

    public void RequestBuildTheBridge(string bridgeSlotId)
    {
        if (player == null || Client == null || !Client.IsConnect) return;
        if (!isFreezingSending)
        {
            Debug.Log("send fill bridge");
            isFreezingSending = true;
            Client.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_BUILD_BRIDGE, bridgeSlotId);
            Invoke(nameof(ResetFreezingSending), sendInterval);
        }
    }

    public void RequestKickTheOtherPlayer(Vector3 moveDirection, string otherPlayerId,Vector3 otherPosition)
    {
        if (player == null || Client == null || !Client.IsConnect) return;
        Debug.Log("kick player");
        var message = new { direction = NetworkUltilityHelper.ConvertFromVector3ToVect3(moveDirection),
            otherplayerId = otherPlayerId,
            otherPlayerPosition = NetworkUltilityHelper.ConvertFromVector3ToVect3(otherPosition)};
        Client.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_KICK_OTHER_PLAYER, message, sendInterval * multiplierDelaySend);
    }

    public void RequestCheckPlayerWin()
    {
        if (player == null || Client == null || !Client.IsConnect) return;
        Debug.Log("Check player win");
        Client.SendMessageToServer(CommandFromClient.COMMAND_CHECK_PLAYER_WIN,null);
    }
    

    #endregion

    private void ResetFreezingSending()
    {
        isFreezingSending = false;
    }
}

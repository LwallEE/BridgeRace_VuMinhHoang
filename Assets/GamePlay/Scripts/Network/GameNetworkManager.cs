using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : Singleton<GameNetworkManager>
{
    public NetworkClient Client { get; private set; }
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private MapNetwork mapGenerate;
    [field:SerializeField] public float LerpPlayerSpeed { get; private set; }

    [SerializeField] private float timeAttempSend = 0.15f;

    [SerializeField] private float multiplierDelaySend = 1f;
  
    private PlayerNetworkController player;

    private Dictionary<string,PlayerNetworkController> otherPlayers;

    private readonly float sendInterval = 100 / 1000f;
    private float sendTimer = 0f;
    private List<IDispose> elementToDispose = new List<IDispose>();
    private bool isFreezingSending;
    private GameNetworkStateEnum gameState;

    public Action<float> OnPingChange;

    // Start is called before the first frame update
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
        if(IsInGameState(GameNetworkStateEnum.GameLoop) || IsInGameState(GameNetworkStateEnum.CountDown))
            mapGenerate.InitMap(data);
    }

    public void Dispose()
    {
        SetGameState(GameNetworkStateEnum.None);
        if (elementToDispose != null)
        {
            foreach (var item in elementToDispose)
            {
                item.Dispose();
            }
       
            elementToDispose.Clear();
        }
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
        LazyPool.Instance.ReleaseAll();
    }

    public bool IsInGameState(GameNetworkStateEnum state)
    {
        return this.gameState == state;
    }

    private void ResetFreezingSending()
    {
        isFreezingSending = false;
    }

    public void PingChange(float ping)
    {
        OnPingChange?.Invoke(ping);
    }

    public void BackToMainMenuScene()
    {
        Debug.Log("back to menu");
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
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

    public void CountDownTextChange(int value)
    {
        UINetworkManager.Instance.GetGamePlayNetworkUI().UpdateText(value);
    }

    public void SetGameState(GameNetworkStateEnum state)
    {
        this.gameState = state;
        Debug.Log("Change game state " + state);
        if (this.gameState == GameNetworkStateEnum.Waiting)
        {
            UINetworkManager.Instance.OpenWaitingRoom();
        }
        else if (this.gameState == GameNetworkStateEnum.CountDown)
        {
            UINetworkManager.Instance.OpenGamePlayPanel();
            UINetworkManager.Instance.GetGamePlayNetworkUI().ActiveCountDownText(true);
        }
        else if (this.gameState == GameNetworkStateEnum.GameLoop)
        {
            UINetworkManager.Instance.OpenGamePlayPanel();
            UINetworkManager.Instance.GetGamePlayNetworkUI().ActiveCountDownText(false);
        }
    }

    public void PlayerJoinRoomWaiting(NetworkUserData user)
    {
        UINetworkManager.Instance.GetWaitingRoom().AddPlayer(user);
        user.OnIsReadyChange((value, previousValue) =>
        {
            UINetworkManager.Instance.GetWaitingRoom().PlayerUIReadyChange(user.userId, value);
            if (user.userId == Client.GameRoomNetwork.GetSessionId())
            {
                UINetworkManager.Instance.GetWaitingRoom().Ready(value);
            }
        });
    }

    public void PlayerLeaveRoomWaiting(string key)
    {
        UINetworkManager.Instance.GetWaitingRoom().RemovePlayer(key);
    }

    public void GameResult(ResultGameResponse response)
    {
        UINetworkManager.Instance.OpenGameResultPanel(response);
        PlayerSaveData.CurrentAchievement = Mathf.Max(0, PlayerSaveData.CurrentAchievement + response.scoreBonusResult);
    }

    public void BackToLobbyRoom()
    {
        UINetworkManager.Instance.GetWaitingPanelUI().Open();
        if (Client != null)
            Client.ConnectToLobbyRoom();
    }
    #endregion
    
    
    #region MessageSendToServer
    //Waiting room
    public void RequestPlayerReadyChange(bool isReady)
    {
        if (Client == null || !Client.GameRoomNetwork.IsConnect) return;
        Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_READY, isReady);
    }
    
    //Gameplay
    public void UpdatePositionRotationOfPlayerToServer()
    {
        if (player == null || Client == null || !Client.GameRoomNetwork.IsConnect || !Client.GameRoomNetwork.CheckRoomAvailable()) return;
        //Compare the last player data's position and current's position
        //if not change return
        
        if (!Client.GameRoomNetwork.IsChangePosition(player.Id, player.transform.position)) return;
        
        PlayerInputMessage message = new PlayerInputMessage()
        {
            position = NetworkUltilityHelper.ConvertFromVector3ToVect3(player.transform.position),
            yRotation = player.transform.eulerAngles.y
        };
       
       
        
           
        Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_PLAYER_POSITION_ROTATION,message);
        
    }

    public void UpdateGreyBrickPosition()
    {
        if (mapGenerate == null || Client == null || !Client.GameRoomNetwork.IsConnect) return;
        var message = mapGenerate.GetGreyBrickMessage();
        if (message.brickChanges == null || message.brickChanges.Count <= 0) return;
        Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_GREY_BRICK, message);
    }

    public void UpdateAnimationOfPlayerToServer(string newState)
    {
        if (player == null || Client == null || !Client.GameRoomNetwork.IsConnect) return;
        Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_UPDATE_PLAYER_ANIMATION, newState);
    }

    public void RequestCollectBrick(string brickId)
    {
        if (player == null || Client == null || !Client.GameRoomNetwork.IsConnect) return;
        Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_COLLECT_BRICK, brickId, sendInterval*multiplierDelaySend);
    }

    public void RequestBuildTheBridge(string bridgeSlotId)
    {
        if (player == null || Client == null || !Client.GameRoomNetwork.IsConnect) return;
        if (!isFreezingSending)
        {
            //Debug.Log("send fill bridge");
            isFreezingSending = true;
            Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_BUILD_BRIDGE, bridgeSlotId);
            Invoke(nameof(ResetFreezingSending), sendInterval);
        }
    }

    public void RequestKickTheOtherPlayer(Vector3 moveDirection, string otherPlayerId,Vector3 otherPosition)
    {
        if (player == null || Client == null || !Client.GameRoomNetwork.IsConnect) return;
        Debug.Log("kick player");
        var message = new { direction = NetworkUltilityHelper.ConvertFromVector3ToVect3(moveDirection),
            otherplayerId = otherPlayerId,
            otherPlayerPosition = NetworkUltilityHelper.ConvertFromVector3ToVect3(otherPosition)};
        Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_PLAYER_KICK_OTHER_PLAYER, message, sendInterval * multiplierDelaySend);
    }

    public void RequestCheckPlayerWin()
    {
        if (player == null || Client == null || !Client.GameRoomNetwork.IsConnect) return;
        Debug.Log("Check player win");
        Client.GameRoomNetwork.SendMessageToServer(CommandFromClient.COMMAND_CHECK_PLAYER_WIN,null);
    }
    

    #endregion

   
}

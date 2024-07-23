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
    [field:SerializeField] public float LerpPlayerSpeed { get; private set; }
    // Start is called before the first frame update
    private PlayerNetworkController player;

    private Dictionary<string,PlayerNetworkController> otherPlayers;

    private readonly float sendInterval = 100 / 1000f;
    private float sendTimer = 0f;

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
    
    #region CallBackToSyncGame
    public void UpdatePositionOfPlayer(string key, Vect3 position)
    {
        var playerr = GetPlayer(key);
        if (playerr == null)
        {
            Debug.LogError("players doesn't contain player with same key");
            return;
        }
        
        playerr.SetDestination(position);
    }

    public void UpdateRotationOfPlayer(string key, float yRotation)
    {
        var playerr = GetPlayer(key);
        if (playerr == null)
        {
            Debug.LogError("players doesn't contain player with same key "+key);
            return;
        }
        playerr.SetYRotation(yRotation);
    }

    public void UpdateAnimNameOfPlayer(string key, string newValue, string previousValue)
    {
        var playerr = GetPlayer(key);
        if (playerr == null)
        {
            Debug.LogError("players doesn't contain player with same key " + key);
            return;
        }
        playerr.SetAnimName(newValue, previousValue);
    }
    #endregion

    #region MessageSendToServer
    public void UpdatePositionRotationOfPlayerToServer()
    {
        if (player == null) return;
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
       
       
        if (client != null)
        {
           
            client.SendUpdatePosition(message);
        }
    }

    public void UpdateAnimationOfPlayerToServer(string newState)
    {
        if (player == null || client == null) return;
        client.SendMessage<string>(CommandFromClient.COMMAND_UPDATE_PLAYER_ANIMATION, newState);
    }
    

    #endregion
   
}

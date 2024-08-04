using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingNetworkUI : UINetworkBase
{
    [SerializeField] private TextMeshProUGUI roomNameTxt;

    [SerializeField] private List<PlayerWaitingUI> listPlayerInfo;

    [SerializeField] private Image readyButton;
    [SerializeField] private TextMeshProUGUI numberOfPlayerCanStartTxt;
    private bool isPlayerReady;
    private int currentNumberOfPlayer;
    private int minNumberOfPlayerToStart;

    public void Init(string roomName,int numberOfPlayerToStart)
    {
        this.roomNameTxt.text = roomName;
        this.minNumberOfPlayerToStart = numberOfPlayerToStart;
        Ready(false);
    }

    public void AddPlayer(NetworkUserData data)
    {
        Debug.Log("current number of Player " + currentNumberOfPlayer);
        if (currentNumberOfPlayer < listPlayerInfo.Count)
        {
            listPlayerInfo[currentNumberOfPlayer].Init(data.userName, (int)data.score, currentNumberOfPlayer+1,data.userId);
            currentNumberOfPlayer++;
            ActivePlayer(currentNumberOfPlayer);
            UpdateVisual();
        }
    }

    public void RemovePlayer(string playerId)
    {
        if (currentNumberOfPlayer > 0)
        {
            var playerWaitingUI = FindPlayerWaitingUIWithId(playerId);
            if (playerWaitingUI == null) return;
            playerWaitingUI.gameObject.SetActive(false);
            currentNumberOfPlayer--;
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        int numberOfPlayer = 0;
        int numberOfPlayerReady = 0;
        foreach (var item in listPlayerInfo)
        {
            if (item.gameObject.activeSelf)
            {
                numberOfPlayer++;
                //update stt
                item.UpdatePlayerNumberText(numberOfPlayer);
                if (item.IsReady)
                {
                    numberOfPlayerReady++;
                }
            }
        }

        numberOfPlayerCanStartTxt.text =
            $"Ready Player : {numberOfPlayerReady}/{Mathf.Max(numberOfPlayer, minNumberOfPlayerToStart)}";
    }
    public void PlayerUIReadyChange(string playerId,bool value)
    {
        Debug.Log("ready change");
        var playerUI = FindPlayerWaitingUIWithId(playerId);
        if(playerUI != null) playerUI.Ready(value);
        
        UpdateVisual();
    }

    private PlayerWaitingUI FindPlayerWaitingUIWithId(string playerId)
    {
        foreach (var item in listPlayerInfo)
        {
            if (item.gameObject.activeSelf && item.PlayerId == playerId)
            {
                return item;
            }
        }

        return null;
    }
    private void ActivePlayer(int number)
    {
        for (int i = 0; i < listPlayerInfo.Count; i++)
        {
            if (i < currentNumberOfPlayer)
            {
                listPlayerInfo[i].gameObject.SetActive(true);
            }
            else
            {
                listPlayerInfo[i].gameObject.SetActive(false);
            }
        }
    }
    public void Ready(bool isReady)
    {
        this.isPlayerReady = isReady;
        var color = readyButton.color;
        if (!isReady)
        {
            color.a = 0.5f;
        }
        else
        {
            color.a = 1f;
        }

        readyButton.color = color;
    }

    public void OnButtonReadyClick()
    {
        if(GameNetworkManager.Instance != null)
            GameNetworkManager.Instance.RequestPlayerReadyChange(!isPlayerReady);
    }

    public void OnLeaveRoomButtonClick()
    {
        if(GameNetworkManager.Instance != null)
            GameNetworkManager.Instance.BackToLobbyRoom();
    }

    private void OnDisable()
    {
        Debug.Log("reset current player");
        currentNumberOfPlayer = 0;
    }
}

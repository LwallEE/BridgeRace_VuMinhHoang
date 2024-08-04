using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UINetworkManager : Singleton<UINetworkManager>
{
    [SerializeField] private WaitingNetworkUI waitingRoomPanel;
    [SerializeField] private GameplayNetworkUI gamePlayPanel;
    [SerializeField] private LobbyRoomNetworkUI lobbyRoomPanel;
    [SerializeField] private WaitingUI waitingPopUp;
    [SerializeField] private PopUpMessageUI popUpMessage;
    [SerializeField] private GameResultPanel gameResultPanel;
    [SerializeField] private UINetworkBase networkErrorPanel;
    public void OpenWaitingRoom()
    {
        if (waitingRoomPanel.IsOpen()) return;
        CloseAll();
        waitingRoomPanel.Open();
        waitingRoomPanel.Init(lobbyRoomPanel.CurrentRoomName, GameNetworkManager.Instance.Client.GameRoomNetwork.GetMinNumberOfPlayerToStart());
    }

    public void OpenGamePlayPanel()
    {
        CloseAll();
        gamePlayPanel.Open();
    }

    public void OpenLobbyRoomPanel()
    {
        if (lobbyRoomPanel.IsOpen()) return;
        CloseAll();
        lobbyRoomPanel.Open();
    }

    public void OpenPopUpMessage(string message, UnityAction closeAction)
    {
        waitingPopUp.Close();
        popUpMessage.Open();
        popUpMessage.InitMessage(message, closeAction);
    }

    public void OpenGameResultPanel(ResultGameResponse response)
    {
        CloseAll();
        gameResultPanel.Open();
        gameResultPanel.Init(response);
    }

    public void OpenNetworkErrorPanel()
    {
        CloseAll();
        networkErrorPanel.Open();
    }

    public WaitingUI GetWaitingPanelUI()
    {
        return waitingPopUp;
    }

    public WaitingNetworkUI GetWaitingRoom()
    {
        return waitingRoomPanel;
    }

    public GameplayNetworkUI GetGamePlayNetworkUI()
    {
        return gamePlayPanel;
    }

    public LobbyRoomNetworkUI GetLobbyPanelUI()
    {
        return lobbyRoomPanel;
    }

    public PopUpMessageUI GetPopUpMessageUI()
    {
        return popUpMessage;
    }
    public void CloseAll()
    {
        waitingRoomPanel.gameObject.SetActive(false);
        gamePlayPanel.gameObject.SetActive(false);
        lobbyRoomPanel.gameObject.SetActive(false);
        waitingPopUp.gameObject.SetActive(false);
        popUpMessage.gameObject.SetActive(false);
        gameResultPanel.gameObject.SetActive(false);
        networkErrorPanel.gameObject.SetActive(false);
    }
    
}

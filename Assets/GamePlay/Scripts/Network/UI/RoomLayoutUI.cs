using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomLayoutUI : MonoBehaviour
{
    [SerializeField] private Image roomAvatarBg;

    [SerializeField] private Image roomAvatarIcon;

    [SerializeField] private TextMeshProUGUI roomNameTxt;

    [SerializeField] private TextMeshProUGUI numberOfPlayerTxt;
    [SerializeField] private Button JoinButton;
    [SerializeField] private TextMeshProUGUI lockTxt;
    public string RoomId { get; private set; }
    public string RoomName { get; private set; }
    public bool IsLock { get; private set; }
    private int currentClient;
    private int maxClient;
    private int avatarIconIndex;
    private int avatarbgIndex;

    public void InitRoom(string roomId, string roomName, int currentClients, int maxClients, int iconIndex, int bgIndex,bool isLock)
    {
        this.RoomId = roomId;
        this.RoomName = roomName;
        this.currentClient = currentClients;
        this.maxClient = maxClients;
        this.avatarbgIndex = bgIndex;
        this.avatarIconIndex = iconIndex;
        this.IsLock = isLock;
        UpdateVisualRoom();
    }

    public void UpdateRoom(string roomName, int currentClients, int maxClients, int iconIndex, int bgIndex, bool isLock)
    {
        this.RoomName = roomName;
        this.currentClient = currentClients;
        this.maxClient = maxClients;
        this.avatarbgIndex = bgIndex;
        this.avatarIconIndex = iconIndex;
        this.IsLock = isLock;
        UpdateVisualRoom();
    }
    private void UpdateVisualRoom()
    {
        this.roomNameTxt.text = this.RoomName;
        this.numberOfPlayerTxt.text = $"{this.currentClient}/{this.maxClient}";
        roomAvatarBg.sprite = GameAssets.Instance.roomAvatarBgSprites[this.avatarbgIndex];
        roomAvatarIcon.sprite = GameAssets.Instance.roomAvatarIconSprites[this.avatarIconIndex];

        bool canJoin = CanJoin();
        lockTxt.gameObject.SetActive(!canJoin);
        JoinButton.gameObject.SetActive(canJoin);
    }

    public bool CanJoin()
    {
        return this.currentClient < this.maxClient && !IsLock;
    }

    public void OnJoinButtonClick()
    {
        if (CanJoin())
        {
            UINetworkManager.Instance.GetLobbyPanelUI().JoinRoom(this);
        }
    }
}

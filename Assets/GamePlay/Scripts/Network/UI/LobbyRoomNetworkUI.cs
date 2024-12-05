using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagerExample;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyRoomNetworkUI : UINetworkBase
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private RectTransform roomContainer;
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private TMP_InputField roomNameField;
    
    private List<RoomLayoutUI> listRoomAvaiable;
    public string CurrentRoomName { get; private set; }
    public void AddOrUpdateRoom(RoomListingData data)
    {
        var room = GetRoom(data.roomId);
        if (room == null)
        {
            //add new room
            AddNewRoom(data);
        }
        else
        {
            //update this room
            room.UpdateRoom(data.metadata.name,data.clients, data.maxClients, data.metadata.iconAvatar,data.metadata.bgAvatar,data.locked);
        }
    }

    public void InitializeListRoom(RoomListingData[] rooms)
    {
        if (listRoomAvaiable != null)
        {
            RemoveAllRoom();
        }
        listRoomAvaiable = new List<RoomLayoutUI>();
        foreach (var room in rooms)
        {
            AddNewRoom(room);
        }
    }

    public void RemoveRoom(string roomId)
    {
        var room = GetRoom(roomId);
        if (room != null)
        {
            listRoomAvaiable.Remove(room);
            LazyPool.Instance.AddObjectToPool(room.gameObject);
        }
    }

    private void RemoveAllRoom()
    {
        foreach (var room in listRoomAvaiable)
        {
            Destroy(room.gameObject);
        }
        listRoomAvaiable.Clear();
    }
    private void AddNewRoom(RoomListingData data)
    {
        if (listRoomAvaiable == null) listRoomAvaiable = new List<RoomLayoutUI>();
        var newRoom = LazyPool.Instance.GetObj<RoomLayoutUI>(roomPrefab);
        newRoom.transform.SetParent(roomContainer);
        newRoom.GetComponent<RectTransform>().localScale = Vector3.one;
        newRoom.InitRoom(data.roomId,data.metadata.name, data.clients,data.maxClients,
            data.metadata.iconAvatar,data.metadata.bgAvatar, data.locked);
        listRoomAvaiable.Add(newRoom);
    }

    private RoomLayoutUI GetRoom(string roomId)
    {
        if (listRoomAvaiable == null) return null;
        foreach (var room in listRoomAvaiable)
        {
            if (room.RoomId == roomId)
            {
                return room;
            }
        }

        return null;
    }

    public void OnCreateButtonClick()
    {
        CreateRoom();
    }

    public void OnExitButtonClick()
    {
        GameNetworkManager.Instance.Client.LeaveLobbyRoom();
        GameController.Instance.BackToMainMenu();
    }

    public void JoinRoom(RoomLayoutUI room)
    {
        /*var userName = userNameField.text;
        if (string.IsNullOrEmpty(userName))
        {
            //<TO DO> pop up message "user name must be filled"
            UINetworkManager.Instance.OpenPopUpMessage("User name must be filled", () =>
            {
                UINetworkManager.Instance.GetPopUpMessageUI().Close();
            });
            return;
        }*/
        JoinRoomAsync(room.RoomId,"");
    }

    private async Task JoinRoomAsync(string roomId,string userName)
    {
        UINetworkManager.Instance.GetWaitingPanelUI().Open();
        await GameNetworkManager.Instance.Client.JoinGameRoom(userName, roomId);
    }
    private async Task CreateRoom()
    {
        var userName = userNameField.text;
        var roomName = roomNameField.text;
        if ( string.IsNullOrEmpty(roomName))
        {
            //<TO DO> Show dialog message here with message {user name and room name must be non empty}
            UINetworkManager.Instance.OpenPopUpMessage("Room's name must be non empty", () =>
            {
                UINetworkManager.Instance.GetPopUpMessageUI().Close();
            });
            
            return;
        }

        bool isRoomExist = CheckRoomExist(roomName);
        if (isRoomExist)
        {
            //<TO DO> show dialog message here with message "Room name has exist, please choose another name"
            UINetworkManager.Instance.OpenPopUpMessage("Room name has exist, please choose another name", () =>
            {
                UINetworkManager.Instance.GetPopUpMessageUI().Close();
            });
            return;
        }

        this.CurrentRoomName = roomName;
        UINetworkManager.Instance.GetWaitingPanelUI().Open();
        await GameNetworkManager.Instance.Client.CreateGameRoom(userName, roomName);
        
    }

    private bool CheckRoomExist(string roomName)
    {
        foreach (var room in listRoomAvaiable)
        {
            if (room.RoomName == roomName)
            {
                return true;
            }
        }

        return false;
    }
}

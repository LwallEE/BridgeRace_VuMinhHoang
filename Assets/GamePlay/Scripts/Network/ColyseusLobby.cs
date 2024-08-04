using System;
using System.Threading.Tasks;
using Colyseus;
using GameDevWare.Serialization;
using System.Reflection;
using UnityEngine;
[Serializable]
public class Metadata
{
    public string name;
    public byte bgAvatar;
    public byte iconAvatar;
}

[Serializable]
public class RoomListingData
{
    public byte clients;
    public bool locked;
    public bool isPrivate;
    public byte maxClients;
    public Metadata metadata;
    public string name;
    public string processId;
    public string roomId;
    public bool unlisted;
}
public class ColyseusLobby
{
    private readonly ColyseusClient _client;

    public ColyseusRoom<dynamic> room;

    public event Action<RoomListingData[]> OnRooms;

    public event Action<string, RoomListingData> OnAddRoom;

    public event Action<string> OnRemoveRoom;

    private string lobbyName;

    public ColyseusLobby(ColyseusClient client,string lobbyName)
    {
        _client = client;
        this.lobbyName = lobbyName;
    }

    public async Task Connect()
    {
      
        room = await _client.JoinOrCreate(lobbyName);
      
        room.OnMessage<RoomListingData[]>("rooms", OnRoomsMessage);
        room.OnMessage<object[]>("+", OnAddRoomMessage);
        room.OnMessage<string>("-", OnRemoveRoomMessage);
    }


    void OnRoomsMessage(RoomListingData[] rooms)
    {
        OnRooms?.Invoke(rooms);
    }

    private T ConvertData<T>(ref T obj, FieldInfo[] fields, IndexedDictionary<string, object> data)
    {
        foreach (FieldInfo field in fields)
        {
            if (data.ContainsKey(field.Name))
            {
                data.TryGetValue(field.Name, out object value);
                if (value is IndexedDictionary<string, object> indexedDictionary)
                {
                    object fieldValue = Activator.CreateInstance(field.FieldType);
                    field.SetValue(obj, ConvertData(ref fieldValue, field.FieldType.GetFields(), indexedDictionary));
                }
                else if (field.FieldType == value.GetType())
                {
                    field.SetValue(obj, value);
                }
            }
        }

        return obj;
    }

    void OnAddRoomMessage(object[] info)
    {
        var roomId = (string)info[0];
        var data = (IndexedDictionary<string, object>)info[1];

        
        
        var roomInfo = new RoomListingData();
        var roomType = typeof(RoomListingData);
        
        
        FieldInfo[] fields = roomType.GetFields();
        
        ConvertData(ref roomInfo, fields, data);

        OnAddRoom?.Invoke(roomId, roomInfo);

    }

    void OnRemoveRoomMessage(string roomId)
    {
        OnRemoveRoom?.Invoke(roomId);
    }

    public async Task LeaveRoom()
    {
        if (room != null)
        {
            await room.Leave(true);
        }
    }
}
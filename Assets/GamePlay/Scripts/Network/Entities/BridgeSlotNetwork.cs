using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class BridgeSlotNetwork : BridgeSlot,IDispose
{
    public string Id { get; private set; }
    private NetworkEventHandler networkEventHandler = new NetworkEventHandler();
    protected override void Start()
    {
        
    }

    public void InitBridgeSlot(BridgeSlotData data)
    {
        transform.position = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.position);
        Id = data.entityId;
        SetColor((BrickColor)data.color);
        
        //event and dispose
        RegisterEventNetwork(data);
        GameNetworkManager.Instance.AddToDisposeList(this);
    }

    private List<Action> RegisterEventNetwork(BridgeSlotData data)
    {
        List<Action> returnList = new List<Action>();
        returnList.Add(data.OnColorChange((value, previousValue) =>
        {
            if (GameNetworkManager.Instance.GetMainPlayer() != null &&
                (BrickColor)value == GameNetworkManager.Instance.GetMainPlayer().GetColor())
            {
                SoundManager.Instance.PlayShot(SoundManager.Instance.GetSoundDataOfType(ESound.BuildBridge, true));
            }
            SetColor((BrickColor)value);
        }));
        return returnList;
    }

    public void Dispose()
    {
        networkEventHandler.UnRegister();
    }
}

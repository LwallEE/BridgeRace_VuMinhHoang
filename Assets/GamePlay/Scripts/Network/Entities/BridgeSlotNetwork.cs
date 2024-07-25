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
        GameNetworkManager.Instance.AddToDisposeList(this);
    }

    public void Dispose()
    {
        networkEventHandler.UnRegister();
    }
}

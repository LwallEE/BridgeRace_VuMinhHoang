using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class BrickNetwork : Brick,IDispose
{
    public string Id { get; private set; }
    private NetworkEventHandler networkEventHandler = new NetworkEventHandler();
    protected override void Update()
    {
        if (brickState == EBrickState.BrickDynamic && !canCollect)
        {
            if (currentTimeCoolDownToCollect > 0)
            {
                currentTimeCoolDownToCollect -= Time.deltaTime;
            }
            else
            {
                canCollect = true;
            }
        }
    }

    public void InitBrickNetwork(BrickData data)
    {
        if (data.state == (int)EBrickState.BrickStatic)
        {
            InitBrickStatic((BrickColor)data.color, NetworkUltilityHelper.ConvertFromVect3ToVector3(data.position));
        }
        else
        {
            InitBrickDynamic(NetworkUltilityHelper.ConvertFromVect3ToVector3(data.position));
        }

        collider.center = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.centerPosition);
        collider.size = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.size);
        this.Id = data.entityId;
        
        //event and dispose
        GameNetworkManager.Instance.AddToDisposeList(this);
    }

    public void Dispose()
    {
        networkEventHandler.UnRegister();
    }
}

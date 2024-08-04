using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickNetwork : Brick,IDispose
{
    public string Id { get; private set; }
    public bool IsMine { get; private set; }
    private NetworkEventHandler networkEventHandler = new NetworkEventHandler();
    private Vector3 targetPosition;
    
    protected override void Update()
    {
        if (brickState == EBrickState.BrickDynamic )
        {
            if (!canCollect)
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

            if (!IsMine)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition,
                    Time.deltaTime * GameNetworkManager.Instance.LerpPlayerSpeed);
            }
        }
    }

    public void InitBrickNetwork(BrickData data)
    {
        IsMine = false;
        if (data.state == (int)EBrickState.BrickStatic)
        {
            InitBrickStatic((BrickColor)data.color, NetworkUltilityHelper.ConvertFromVect3ToVector3(data.position));
        }
        else
        {
            if (data.ownerId == GameNetworkManager.Instance.Client.GameRoomNetwork.GetSessionId())
            {
                IsMine = true;
            }
            InitBrickDynamic(NetworkUltilityHelper.ConvertFromVect3ToVector3(data.position));
            targetPosition = transform.position;
        }

        collider.center = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.centerPosition);
        collider.size = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.size);
        this.Id = data.entityId;
        
        //event and dispose
        networkEventHandler.InitEventRegister(RegisterEventNetwork(data));
        GameNetworkManager.Instance.AddToDisposeList(this);
    }

    public override void InitBrickDynamic(Vector3 position)
    {
        if (IsMine)   //if is mine, the client has the responsibility to simulate the physic of this brick
        {        
            base.InitBrickDynamic(position);
        }
        else
        {
            transform.position = position;
            brickState = EBrickState.BrickDynamic;
            canCollect = false;
            Active(true);
            ActiveRigibody(false);
            SetColor(GameAssets.Instance.GetColorData(BrickColor.Grey));
            //time wait for collect
            currentTimeCoolDownToCollect = Constants.BRICK_COOLDOWN_TIME_TO_COLLECT;
        }
    }

    private List<Action> RegisterEventNetwork(BrickData data)
    {
        List<Action> result = new List<Action>();
        result.Add(data.OnIsActiveChange((value, previousValue) =>
        {
            Active(value);
        })
        );
        if (brickState == EBrickState.BrickDynamic && !IsMine)
        {
            result.Add(data.OnPositionChange((value, previousValue) =>
            {
//                Debug.Log("brick change position");
                targetPosition = NetworkUltilityHelper.ConvertFromVect3ToVector3(value);
            }));
        }
        return result;
    }
    public void Dispose()
    {
        networkEventHandler.UnRegister();
    }
}

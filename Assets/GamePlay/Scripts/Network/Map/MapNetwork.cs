using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class MapNetwork : MonoBehaviour
{
    [SerializeField] private GameObject stageNetworkPrefab;
    [SerializeField] private GameObject winPosPrefab;
    [SerializeField] private GameObject brickNetworkPrefab;
    
    private NetworkEventHandler networkEventHandler = new NetworkEventHandler();
    private List<StageNetwork> stageNetworks;
    private Transform winPos;
    private List<BrickNetwork> greyBrickNetwork = new List<BrickNetwork>();
    

    public void InitMap(MapData data)
    {
        stageNetworks = new List<StageNetwork>();
        int i = 0;
        data.stages.ForEach(stage =>
        {
            var stageElement = Instantiate(stageNetworkPrefab, transform).GetComponent<StageNetwork>();
            stageElement.InitStage(stage);
            stageNetworks.Add(stageElement);
            int numberOfBridge = i == 0 ? 0 : stageNetworks[i - 1].GetNumberOfBridge();
            stageElement.UpdateDownCollider(numberOfBridge);
            i++;
        });
        winPos = Instantiate(winPosPrefab, transform).transform;
        winPos.transform.position = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.winPosition);
        networkEventHandler.InitEventRegister(RegisterEventNetwork(data));
    }

    private List<Action> RegisterEventNetwork(MapData data)
    {
        List<Action> returnAction = new List<Action>();
        returnAction.Add(data.greyBricks.OnAdd((key, value) =>
        {
            var greyBrick = LazyPool.Instance.GetObj<BrickNetwork>(brickNetworkPrefab);
            greyBrick.InitBrickNetwork(value);
            greyBrickNetwork.Add(greyBrick);
        }));
        returnAction.Add(data.greyBricks.OnRemove((key, value) =>
        {
            var brick = GetGreyBrick(key);
            if (brick != null)
            {
                brick.Dispose();
                greyBrickNetwork.Remove(brick);
                LazyPool.Instance.AddObjectToPool(brick.gameObject);
            }
        } ));
        return returnAction;
    }
    public void Dispose()
    {
        if (stageNetworks != null)
        {
            foreach (var item in stageNetworks)
            {
                Destroy(item.gameObject);
            }
            stageNetworks.Clear();
        }
        networkEventHandler.UnRegister();
        if(winPos != null)
            Destroy(winPos.gameObject);
        if (greyBrickNetwork != null)
        {
            foreach (var item in greyBrickNetwork)
            {
                Destroy(item.gameObject);
            }
            greyBrickNetwork.Clear();
        }
    }

    private BrickNetwork GetGreyBrick(string id)
    {
        if (greyBrickNetwork == null) return null;
        foreach (var item in greyBrickNetwork)
        {
            if (item.Id == id)
            {
                return item;
            }
        }

        return null;
    }
    public GreyBrickPositionMessage GetGreyBrickMessage()
    {
        GreyBrickPositionMessage message = new GreyBrickPositionMessage();
        message.brickChanges = new List<BrickPositionMessage>();
        foreach (var brick in greyBrickNetwork)
        {
            if (brick.IsMine && GameNetworkManager.Instance.Client.GameRoomNetwork.IsChangePosition(brick.Id, brick.transform.position))
            {
                message.brickChanges.Add(new BrickPositionMessage()
                {
                    key = brick.Id,
                    position = NetworkUltilityHelper.ConvertFromVector3ToVect3(brick.transform.position)
                });
            }
           
        }

        return message;
    }
}

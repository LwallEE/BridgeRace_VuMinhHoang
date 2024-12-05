using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class StageNetwork : MonoBehaviour
{
    [SerializeField] private GameObject bridgePrefab;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Transform brickContainer;
    [SerializeField] private List<GameObject> downBoxCollider;
    [SerializeField] private List<GameObject> decorations;
    
    private List<BridgeNetwork> bridgeNetworks;
    private Dictionary<string,BrickNetwork> brickNetworks;
    public void InitStage(StageData data)
    {
        transform.position = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.stagePosition);
        bridgeNetworks = new List<BridgeNetwork>();
        brickNetworks = new Dictionary<string, BrickNetwork>();
        
        //spawn bridge
        data.bridges.ForEach(bridge =>
        {
            var bridgeElement = Instantiate(bridgePrefab, transform).GetComponent<BridgeNetwork>();
            bridgeElement.InitBridge(bridge);
            bridgeNetworks.Add(bridgeElement);
        });
        //spawn brick
        data.bricks.ForEach((key, brickData) =>
        {
            var brickElement = Instantiate(brickPrefab, brickContainer).GetComponent<BrickNetwork>();
            brickElement.InitBrickNetwork(brickData);
            brickNetworks.TryAdd(key, brickElement);
        } );
    }

    public void UpdateDownCollider(int number)
    {
        for (int i = 0; i < downBoxCollider.Count; i++)
        {
            if (i == number)
            {
                downBoxCollider[i].SetActive(true);
            }
            else
            {
                downBoxCollider[i].SetActive(false);
            }
        }
    }

    public void UpdateDecorations(int index)
    {
        for (int i = 0; i < decorations.Count; i++)
        {
           decorations[i].SetActive(i == index);
        }
    }

    public int GetNumberOfBridge()
    {
        return bridgeNetworks.Count;
    }
}

using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class MapNetwork : MonoBehaviour
{
    [SerializeField] private GameObject stageNetworkPrefab;
    [SerializeField] private GameObject winPosPrefab;
    
    private List<StageNetwork> stageNetworks;
    private Transform winPos;

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
    }

    public void Dispose()
    {
        foreach (var item in stageNetworks)
        {
            Destroy(item.gameObject);
        }
        stageNetworks.Clear();
        Destroy(winPos.gameObject);
    }
}

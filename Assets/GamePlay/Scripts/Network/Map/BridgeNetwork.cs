using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class BridgeNetwork : MonoBehaviour
{
    [SerializeField] private GameObject bridgeSlotPrefab;
    [SerializeField] private Transform bridgeContainer;
    [SerializeField] private StairCollider stairCollider;
    [SerializeField] private BoxCollider leftCollider;
    [SerializeField] private BoxCollider rightCollider;
    private Dictionary<string, BridgeSlotNetwork> bridgeSlotNetworks;

    public void InitBridge(BridgeData data)
    {
        bridgeSlotNetworks = new Dictionary<string, BridgeSlotNetwork>();
        transform.position = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.bridgePosition);
        data.bridgeSlots.ForEach((key, slotData) =>
        {
            var bridgeSlotElement = Instantiate(bridgeSlotPrefab, bridgeContainer).GetComponent<BridgeSlotNetwork>();
            bridgeSlotElement.InitBridgeSlot(slotData);
            bridgeSlotNetworks.TryAdd(key, bridgeSlotElement);
        } );
        stairCollider.UpdateCollider(bridgeSlotNetworks.Count);
        UpdateLeftRightBounder(bridgeSlotNetworks.Count);
    }

    private void UpdateLeftRightBounder(int number)
    {
        Vector3 newCenter = new Vector3(0, number * 1f / 2 + 1, number * 1f / 2);
        leftCollider.center = new Vector3(leftCollider.center.x, newCenter.y, newCenter.z);
        
        leftCollider.size = new Vector3(leftCollider.size.x, number + 1, number + 1);
        
        rightCollider.center = new Vector3(rightCollider.center.x, newCenter.y, newCenter.z);
        rightCollider.size = new Vector3(rightCollider.size.x, number+1, number+1);
    }

}

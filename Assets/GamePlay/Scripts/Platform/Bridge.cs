using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private List<BridgeSlot> listBridgeSlot;
    [SerializeField] private Transform footOfTheBridge;
    public int GetNumberOfBrickToFinishBridge(BrickColor brickColor)
    {
        int result = 0;
        foreach (var bridge in listBridgeSlot)
        {
            if (!bridge.IsMatchColor(brickColor))
            {
                result++;
            }
        }

        return result;
    }

    public int GetNumberOfBrickHasColor(BrickColor brickColor)
    {
        int result = 0;
        foreach (var bridge in listBridgeSlot)
        {
            if (bridge.IsMatchColor(brickColor))
            {
                result++;
            }
        }

        return result;
    }

    public BridgeSlot GetBridgeIndex(int id)
    {
        if (id >= 0 && id < listBridgeSlot.Count)
        {
            return listBridgeSlot[id];
        }

        return null;
    }

    public Vector3 GetTheInitialPositionOfBridge()
    {
        return footOfTheBridge.transform.position;
    }

    public int GetNumberOfBridgeSlot()
    {
        return listBridgeSlot.Count;
    }
}

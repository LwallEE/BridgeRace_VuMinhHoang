using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
   [SerializeField] private List<Bridge> bridgeList;

   [SerializeField] private GridSpawnBrick gridSpawnBrick;
   
   public Bridge GetOptimalBridge(BrickColor color)
   {
      var result = bridgeList[0];
      foreach (var item in bridgeList)
      {
         if (item.GetNumberOfBrickToFinishBridge(color) < result.GetNumberOfBrickToFinishBridge(color))
         {
            
            result = item;
         }
         else if (item.GetNumberOfBrickHasColor(BrickColor.None) > result.GetNumberOfBrickHasColor(BrickColor.None))
         {
            result = item;
         }
      }

      return result;
   }

   public Brick GetNearestBrickOfColorCanCollect(BrickColor color,Vector3 position)
   {
      return gridSpawnBrick.GetBrickOfColorCanCollect(color,position);
   }

   public int GetMinimumBridgeSlotOfBridge()
   {
      return bridgeList.Min(x => x.GetNumberOfBridgeSlot());
   }

   public int GetMinimumBridgeSlotOfBridgeToFinish(BrickColor color)
   {
      return bridgeList.Min(x => x.GetNumberOfBrickToFinishBridge(color));
   }
}

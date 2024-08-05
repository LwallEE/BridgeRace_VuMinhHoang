using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
   [SerializeField] private List<Map> mapList;
   private Map currentLevel;
   private int currentLevelIndex;

   private void LoadLevel(int level)
   {
      if (level >= mapList.Count) return;
      
      if (currentLevel != null)
      {
         Destroy(currentLevel.gameObject);
         LazyPool.Instance.ReleaseAll();
         currentLevel = null;
      }

      currentLevel = Instantiate(mapList[level].gameObject).GetComponent<Map>();
      currentLevelIndex = level;
      PlayerSaveData.CurrentLevelIndex = level;
   }

   public void LoadNextLevel()
   {
      currentLevelIndex = (currentLevelIndex + 1) % mapList.Count;
      LoadLevel(currentLevelIndex);
   }

   public void LoadCurrentSaveLevel()
   {
      LoadLevel(PlayerSaveData.CurrentLevelIndex);
   }

   public void RestartLevel()
   {
      LoadLevel(currentLevelIndex);
   }
}

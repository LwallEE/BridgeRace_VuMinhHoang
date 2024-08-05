using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUICanvas : UICanvas
{
   public void OnRestartButtonClick()
   {
      GameController.Instance.RestartLevel();
   }

   public void OnContinueButtonClick()
   {
      GameController.Instance.SetGameState(GameState.GameStart);
   }

   public void OnBackToMainMenuClick()
   {
      GameController.Instance.BackToMainMenu();
   }
}

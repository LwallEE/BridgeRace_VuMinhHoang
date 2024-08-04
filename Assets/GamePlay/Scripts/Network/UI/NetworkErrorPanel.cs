using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkErrorPanel : UINetworkBase
{
   public void OnReconnectClick()
   {
      UINetworkManager.Instance.GetWaitingPanelUI().Open();
      GameNetworkManager.Instance.Client.GameRoomNetwork.Reconnect();
   }
}

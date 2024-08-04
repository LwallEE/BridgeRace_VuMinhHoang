using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayNetworkUI : UINetworkBase
{
   [SerializeField] private TextMeshProUGUI countDownTxt;
   [SerializeField] private WifiUI wifiUI;

   private void Start()
   {
      GameNetworkManager.Instance.OnPingChange += wifiUI.UpdateWifiStatus;
   }

   private void OnDestroy()
   {
      GameNetworkManager.Instance.OnPingChange -= wifiUI.UpdateWifiStatus;
   }

   public void UpdateText(int number) //number in milisecond
   {
      int time = number / 1000;
      if (time <= 0)
      {
         countDownTxt.text = "GO";
      }
      else
      {
         countDownTxt.text = time.ToString();
      }
   }

   public void ActiveCountDownText(bool isActive)
   {
      countDownTxt.gameObject.SetActive(isActive);
   }
}

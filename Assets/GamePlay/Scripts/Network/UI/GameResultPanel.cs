using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPanel : UINetworkBase
{
   [SerializeField] private TextMeshProUGUI winLoseTxt;
   [SerializeField] private TextMeshProUGUI winPlayerNameTxt;
   [SerializeField] private TextMeshProUGUI scorePlayerTxt;
   [SerializeField] private TextMeshProUGUI scoreResultTxt;
   [SerializeField] private Image winLoseImage;

   [SerializeField] private Color winColor;
   [SerializeField] private Color loseColor;

   public void Init(ResultGameResponse response)
   {
      if (response.isWin)
      {
         winLoseTxt.text = "WIN";
         winLoseImage.color = winColor;
         
      }
      else
      {
         winLoseTxt.text = "LOSE";
         winLoseImage.color = loseColor;
      }

      scoreResultTxt.text = response.scoreBonusResult.ToString();
      var user = GameNetworkManager.Instance.Client.GameRoomNetwork.GetNetworkUserData(response.winUserId);
      if (user == null) return;
      winPlayerNameTxt.text = user.userName;
      scorePlayerTxt.text = (response.isWin ? "+" : "") + user.score;
   }

   public void OnBackButtonClick()
   {
      if(GameNetworkManager.Instance != null)
         GameNetworkManager.Instance.BackToLobbyRoom();
   }
}

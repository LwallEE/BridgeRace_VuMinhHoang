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
   [SerializeField] private Image playerAvatarImage;
   [SerializeField] private Image winLoseImage;

   [SerializeField] private AvatarData avatarData;
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

      scoreResultTxt.text = (response.scoreBonusResult > 0 ? "+" : "") + response.scoreBonusResult.ToString();
      var user = GameNetworkManager.Instance.Client.GameRoomNetwork.GetNetworkUserData(response.winUserId);
      if (user == null) return;
      int avatarType = 0;
      if (!int.TryParse(user.avatar, out avatarType))
      {
         avatarType = 0;
      }

      playerAvatarImage.sprite = avatarData.GetAvatarByType((AvatarType)avatarType);
      winPlayerNameTxt.text = user.userName;
      scorePlayerTxt.text = user.score.ToString();
   }

   public void OnBackButtonClick()
   {
      if(GameNetworkManager.Instance != null)
         GameNetworkManager.Instance.BackToLobbyRoom();
   }
}

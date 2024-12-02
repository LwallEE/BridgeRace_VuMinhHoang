
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUICanvas : UICanvas
{
    [SerializeField] private GameObject panelWin, panelLose;
    [SerializeField] private PlayerInfor playerInfor;
    [SerializeField] private TextMeshProUGUI txtCoin;
    private bool isWinning;
    public async void Init(bool isWin)
    {
        isWinning = isWin;
        panelLose.SetActive(!isWinning);
        panelWin.SetActive(isWinning);

        var result = await NetworkClient.Instance.HttpGet<UserInfoResponse>("user-info");
        if (result.isSuccess)
        {
            int avatarType = 0;
            try
            {
                avatarType = int.Parse(result.avatar);
            }
            catch { }
            playerInfor.OnInit(result.userName, result.currentCoin, (AvatarType)avatarType);
        }

        if (isWin)
        {
            var result2 = await NetworkClient.Instance.HttpPost<PassLevelResponse>("user/pass-current-level");
            if (result2.isSuccess)
            {
                txtCoin.text = result2.coinGet.ToString();
                playerInfor.AddCoin(result2.coinGet);
            }
        }
    }

    public void OnNextLevelClick()
    {
        if (isWinning)
        {
            GameController.Instance.NextLevel();
        }
        else
        {
            GameController.Instance.RestartLevel();
        }
    }

    public void OnBackToMainMenuClick()
    {
        GameController.Instance.BackToMainMenu();
    }
}

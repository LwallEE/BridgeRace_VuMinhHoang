using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUICanvas : UICanvas
{
    [SerializeField] private GameObject panelWin, panelLose;
    private bool isWinning;
    public void Init(bool isWin)
    {
        isWinning = isWin;
        panelLose.SetActive(!isWinning);
        panelWin.SetActive(isWinning);
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

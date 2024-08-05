using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUICanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI winLoseTxt;
    [SerializeField] private Image winLoseImage;

    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private TextMeshProUGUI btnNextLevelTxt;
    private bool isWinning;
    public void Init(bool isWin)
    {
        isWinning = isWin;
        if (isWin)
        {
            winLoseTxt.text = "WIN";
            winLoseImage.color = winColor;
            btnNextLevelTxt.text = "Next Level";
        }
        else
        {
            winLoseTxt.text = "LOSE";
            winLoseImage.color = loseColor;
            btnNextLevelTxt.text = "Replay Level";
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

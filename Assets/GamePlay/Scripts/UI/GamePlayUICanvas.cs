using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayUICanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI txtTimer;

    public override void Setup()
    {
        base.Setup();
        levelTxt.text = $"Level {(PlayerSaveData.CurrentLevelIndex+1)}";
    }

    public void OnSettingButtonClick()
    {
        GameController.Instance.SetGameState(GameState.GamePause);
    }

    public void SetTimer(string time)
    {
        txtTimer.gameObject.SetActive(true);
        txtTimer.text = time;
    }
}

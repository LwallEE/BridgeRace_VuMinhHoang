using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayUICanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelTxt;

    public override void Setup()
    {
        base.Setup();
        levelTxt.text = $"Level {(PlayerSaveData.CurrentLevelIndex+1)}";
    }

    public void OnSettingButtonClick()
    {
        GameController.Instance.SetGameState(GameState.GamePause);
    }
}

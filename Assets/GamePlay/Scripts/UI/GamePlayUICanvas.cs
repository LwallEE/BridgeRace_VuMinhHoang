using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayUICanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] GameObject[] hpDisplay;
    private int hp;
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
    public void InitHp()
    {
        hp = hpDisplay.Length;
        for (int i = 0; i < hpDisplay.Length; i++)
        {
            hpDisplay[i].SetActive(true);
        }

        txtTimer.gameObject.SetActive(false);
    }
    public void OnHit()
    {
        if (hp <= 0) return;
        hp--;
        hpDisplay[hp].SetActive(false);
        if (hp <= 0) GameController.Instance.SetGameState(GameState.GameEndLose);
    }
}

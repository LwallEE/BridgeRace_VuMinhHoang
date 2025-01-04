using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    GamePlayUICanvas gamePlayUICanvas;
    float timer;
    private void Start()
    {
        gamePlayUICanvas = FindAnyObjectByType<GamePlayUICanvas>();
        timer = 300f;
        gamePlayUICanvas.SetTimer(TimerToString());
    }
    private void Update()
    {
        if (!GameController.Instance.IsInState(GameState.GameStart)) return;
        timer -= Time.deltaTime;
        gamePlayUICanvas.SetTimer(TimerToString());

        if(timer <= 0)
        {
            GameController.Instance.SetGameState(GameState.GameEndLose);
        }
    }
    private string TimerToString()
    {
        int minute = (int)(timer / 60f); 
        int second = (int)(timer % 60f); 

        return $"{minute:D2}:{second:D2}";
    }
}

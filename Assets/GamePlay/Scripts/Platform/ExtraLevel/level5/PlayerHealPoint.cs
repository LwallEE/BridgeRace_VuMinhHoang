using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealPoint : MonoBehaviour
{
    GamePlayUICanvas gamePlayUICanvas;
    private void Start()
    {
        gamePlayUICanvas = FindAnyObjectByType<GamePlayUICanvas>();
        gamePlayUICanvas.InitHp();
    }
    public void OnHit()
    {
        gamePlayUICanvas?.OnHit();
    }
}

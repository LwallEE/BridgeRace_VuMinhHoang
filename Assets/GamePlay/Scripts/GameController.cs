using System;
using System.Collections;
using System.Collections.Generic;
using StateMachineNP;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    GameStart,
    GamePause,
    GameEndWin,
    GameEndLose
}
public class GameController : Singleton<GameController>
{
    [SerializeField] private CameraFollow camera;
    private GameState currentGameState;
    private PlayerController mainPlayer;
    
    private void Start()
    {
        StartGame();
    }

    private void SetUpPlayer()
    {
        mainPlayer = FindObjectOfType<PlayerController>();
        camera.SetTarget(mainPlayer.transform);
        mainPlayer.InitPlayerReference(FindObjectOfType<FloatingJoystick>());
    }

    private void StartGame()
    {
        LevelManager.Instance.LoadCurrentSaveLevel();
        SetGameState(GameState.GameStart);
        SetUpPlayer();
    }
    
    public void SetGameState(GameState state)
    {
        if (currentGameState == state) return;
        currentGameState = state;

        if (currentGameState == GameState.GameStart)
        {
            Debug.Log("open Gameplay UI");
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<GamePlayUICanvas>();
        }
        else if (currentGameState == GameState.GameEndWin)
        {
            var gameResultUI = UIManager.Instance.OpenUI<GameResultUICanvas>(2f);
            gameResultUI.Init(true);
        }
        else if (currentGameState == GameState.GameEndLose)
        {
            var gameResultUI = UIManager.Instance.OpenUI<GameResultUICanvas>();
            gameResultUI.Init(false);
        }
        else if (currentGameState == GameState.GamePause)
        {
            UIManager.Instance.OpenUI<SettingUICanvas>();
        }
    }

    public bool IsInState(GameState state)
    {
        return currentGameState == state;
    }

    public void NextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
        SetGameState(GameState.GameStart);
        SetUpPlayer();
    }

    public void RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
        SetGameState(GameState.GameStart);
        SetUpPlayer();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }
}

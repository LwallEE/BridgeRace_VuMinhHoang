using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StateMachineNP;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    Home,
    GameStart,
    GamePause,
    GameEndWin,
    GameEndLose
}
public class GameController : Singleton<GameController>
{
    [SerializeField] private CameraFollow camera;
    [SerializeField] private GameState currentGameState;
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
        if(currentGameState == GameState.None)
        {
            LevelManager.Instance.LoadCurrentSaveLevel();
            SetGameState(GameState.GameStart);
            SetUpPlayer();
        }
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

    //---------------------HOME---------------------------
    public enum CameraState
    {
        Home, 
        Shop,
    }
    [SerializeField] Transform[] cameraPos;
    public void ChangeCameraState(CameraState state)
    {
        Transform target = cameraPos[(int)state];
        Camera.main.transform.DOMove(target.position, .5f);
        Camera.main.transform.DORotate(target.eulerAngles, .5f);
    }
    //----------------------------------------------------
}

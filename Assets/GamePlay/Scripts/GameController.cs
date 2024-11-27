using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StateMachineNP;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
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
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeGameMenu();
    }

    private void SetUpPlayer()
    {
        mainPlayer = FindObjectOfType<PlayerController>();
        if (camera == null) camera = FindObjectOfType<CameraFollow>();
        camera.SetTarget(mainPlayer.transform);
        mainPlayer.InitPlayerReference(FindObjectOfType<FloatingJoystick>());
    }

    private void OnStartGame()
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
            SoundManager.Instance.PlayShotOneTime(ESound.PlayerWin);
            var gameResultUI = UIManager.Instance.OpenUI<GameResultUICanvas>(2f);
            gameResultUI.Init(true);
        }
        else if (currentGameState == GameState.GameEndLose)
        {
            SoundManager.Instance.PlayShotOneTime(ESound.PlayerLose);
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
        currentGameState = GameState.Home;
        StartCoroutine(LoadMainMenuCoroutine());
    }

    IEnumerator LoadMainMenuCoroutine()
    {
        yield return SceneManager.LoadSceneAsync(Constants.MAIN_MENU_SCENE);
        InitializeGameMenu();
    }

    private void InitializeGameMenu()
    {
        SoundManager.Instance.PlayLoop(ESound.GameMusic);

        
        if (NetworkClient.Instance.IsLogged)
        {
            //open game home canvas here
        }
        else
        {
            UIManager.Instance.OpenUI<LoginUICanvas>();
        }


    }

    public void ChangeToOfflineGame()
    {
        StartCoroutine(ChangeToOfflineGameCoroutine());
    }

    IEnumerator ChangeToOfflineGameCoroutine()
    {
        yield return SceneManager.LoadSceneAsync(Constants.GAME_OFFLINE_SCENE);
        OnStartGame();
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

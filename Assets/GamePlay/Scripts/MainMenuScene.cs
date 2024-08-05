using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void OnPlayOnlineClick()
    {
        SceneManager.LoadScene(Constants.GAME_ONLINE_SCENE);
    }

    public void OnPlayOfflineClick()
    {
        SceneManager.LoadScene(Constants.GAME_OFFLINE_SCENE);
    }
}

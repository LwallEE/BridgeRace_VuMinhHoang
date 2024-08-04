using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 55;
    }

    public void OnPlayOnlineClick()
    {
        SceneManager.LoadScene(Constants.GAME_ONLINE_SCENE);
    }
}

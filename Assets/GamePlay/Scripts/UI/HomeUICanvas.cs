using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUICanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI txtHighScoreHightlight;

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(txtHighScoreHightlight.DOColor(Color.red, 1f));
        sequence.Append(txtHighScoreHightlight.DOColor(Color.white, 1f));
        sequence.SetLoops(-1, LoopType.Restart);
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

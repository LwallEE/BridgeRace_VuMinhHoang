using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUICanvas : UICanvas
{
    [SerializeField] private TextMeshProUGUI txtHighScoreHightlight;
    [SerializeField] private TextMeshProUGUI txtName, txtHightScore, txtCoin;
    [SerializeField] CharacterVisual characterVisual;

    private void Start()
    {
        characterVisual = FindAnyObjectByType<CharacterVisual>();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(txtHighScoreHightlight.DOColor(Color.red, 1f));
        sequence.Append(txtHighScoreHightlight.DOColor(Color.white, 1f));
        sequence.SetLoops(-1, LoopType.Restart);
    }
    public async Task InitUserData()
    {
        await GetUserData();
    }
    private async Task GetUserData()
    {
        var result = await NetworkClient.Instance.HttpGet<UserInfoResponse>("user-info");
        if (result.isSuccess)
        {
            txtCoin.text = result.currentCoin.ToString();
            txtHightScore.text = result.currentPoint.ToString();
            txtName.text = result.userName;

            characterVisual.ChangeAllSkin(result.hatEquippedId, result.pantEquippedId, result.leftHandEquippedId);
        }
    }
    public void OnPlayOnlineClick()
    {
        SceneManager.LoadScene(Constants.GAME_ONLINE_SCENE);
    }

    public void OnPlayOfflineClick()
    {
        SceneManager.LoadScene(Constants.GAME_OFFLINE_SCENE);
    }
    public void OnShopButtonClick()
    {
        GameController.Instance.ChangeCameraState(GameController.CameraState.Shop);
        UIManager.Instance.OpenUI<ShopUICanvas>().InitPlayerInfor(txtName.text, int.Parse(txtCoin.text));
        CloseDirectly();
    }
    public void OnSettingButtonClick()
    {
        UIManager.Instance.OpenUI<SettingUICanvas>();
    }
}

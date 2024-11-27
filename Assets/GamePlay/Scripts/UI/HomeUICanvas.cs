using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    // -----------------------------User Data----------------------------------
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

            int avatarType = 0;
            try
            {
                avatarType = int.Parse(result.avatar);
            }
            catch { }
            currentAvatarType = (AvatarType)avatarType;
            ChangeAvatar(currentAvatarType);
        }
    }
    // ------------------------------------------------------------------------


    // ------------------------------Avatar------------------------------------
    [SerializeField] private Image avatar;
    [SerializeField] private AvatarData avatarData;
    [SerializeField] AvatarFrame[] avatarFrames;

    AvatarType currentAvatarType;

    public void OnAvartarSelected(AvatarType avatarType)
    {
        if (avatarType == currentAvatarType) return;

        avatarFrames[(int)currentAvatarType].OnUnselected();
        ChangeAvatar(avatarType);
    }
    private void ChangeAvatar(AvatarType avatarType)
    {
        currentAvatarType = avatarType;
        avatar.sprite = avatarData.GetAvatarByType(avatarType);
    }
    // ------------------------------------------------------------------------


    #region button click
    public void OnPlayOnlineClick()
    {
        PlayButtonSfx();

        SceneManager.LoadScene(Constants.GAME_ONLINE_SCENE);
    }

    public void OnPlayOfflineClick()
    {
        PlayButtonSfx();

        GameController.Instance.ChangeToOfflineGame();
    }
    public void OnShopButtonClick()
    {
        PlayButtonSfx();

        GameController.Instance.ChangeCameraState(GameController.CameraState.Shop);
        UIManager.Instance.OpenUI<ShopUICanvas>().InitPlayerInfor(txtName.text, int.Parse(txtCoin.text));
        CloseDirectly();
    }
    public void OnSettingButtonClick()
    {
        PlayButtonSfx();
        UIManager.Instance.OpenUI<SettingUICanvas>();
    }
    public void OnRankingButtonClick()
    {
        PlayButtonSfx();
        UIManager.Instance.OpenUI<RankingUICanvas>();
    }
    private void PlayButtonSfx()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);
    }
    #endregion
}

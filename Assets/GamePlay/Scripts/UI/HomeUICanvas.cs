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
    [SerializeField] private TextMeshProUGUI txtHightScore;
    [SerializeField] private PlayerInfor playerInfor;

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
    public void OnEnable()
    {
        InitUserData();
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
            txtHightScore.text = result.currentPoint.ToString();

            characterVisual.ChangeAllSkin(result.hatEquippedId, result.pantEquippedId, result.leftHandEquippedId);

            int avatarType = 0;
            try
            {
                avatarType = int.Parse(result.avatar);
            }
            catch { }
            currentAvatarType = (AvatarType)avatarType;
            playerInfor.OnInit(result.userName, result.currentCoin, currentAvatarType);
            avatarFrames[avatarType].OnFocus();

        }
    }
    // ------------------------------------------------------------------------


    // ------------------------------Avatar------------------------------------
    [SerializeField] GameObject panelAvatar;
    [SerializeField] AvatarFrame[] avatarFrames;

    AvatarType currentAvatarType;

    public async void OnAvartarSelected(AvatarType avatarType)
    {
        if (avatarType == currentAvatarType) return;

        var result = await NetworkClient.Instance.HttpPost<GeneralResponse>("user-info/avatar", new SetAvatarRequest((int)avatarType + ""));
        if (result.isSuccess) {
            avatarFrames[(int)currentAvatarType].OnUnselected();
            ChangeAvatar(avatarType);
        }
        else
        {
            Debug.LogError(result.message);
        }
    }
    private void ChangeAvatar(AvatarType avatarType)
    {
        currentAvatarType = avatarType;
        playerInfor.ChangeAvatar(currentAvatarType);
    }
    // ------------------------------------------------------------------------


    #region Button click
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
        UIManager.Instance.OpenUI<ShopUICanvas>().InitPlayerInfor(playerInfor);
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
    public void OnAvatarClick()
    {
        PlayButtonSfx();
        panelAvatar.SetActive(true);
    }
    public void OnCloseAvatarPanel()
    {
        PlayButtonSfx();
        panelAvatar.SetActive(false);
    }
    private void PlayButtonSfx()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);
    }
    #endregion
}

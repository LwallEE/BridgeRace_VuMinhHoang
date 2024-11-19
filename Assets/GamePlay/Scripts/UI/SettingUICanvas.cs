using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUICanvas : UICanvas
{
    [SerializeField] GameObject credit, button;
    [SerializeField] Image soundImage, musicImage;
    [SerializeField] Sprite[] soundSprites, musicSprites;
    [SerializeField] Slider soundSlider, musicSlider;

    private bool isInHome => GameController.Instance.IsInState(GameState.Home);

    public override void Open()
    {
        base.Open();

        credit.SetActive(isInHome);
        button.SetActive(!isInHome);

        soundSlider.value = SoundManager.Instance.SoundVolume;
        musicSlider.value = SoundManager.Instance.MusicVolume;

        OnSoundVolumeChange();
        OnMusicVolumeChange();
    }
    public void OnRestartButtonClick()
   {
        PlayButtonSfx();

        GameController.Instance.RestartLevel();
   }

   public void OnContinueButtonClick()
   {
        PlayButtonSfx();

        if (isInHome)
        {
            CloseDirectly();
        }
        else
        {
            GameController.Instance.SetGameState(GameState.GameStart);
        }
    }

   public void OnBackToMainMenuClick()
   {
        PlayButtonSfx();
        if (!isInHome)
        {
            GameController.Instance.BackToMainMenu();
        }
        else
        {
            CloseDirectly();
        }
    }
    public void OnSoundVolumeChange()
    {
        float volume = soundSlider.value;
        soundImage.sprite = volume <= 0 ? soundSprites[0] : soundSprites[1];
        SoundManager.Instance.ChangeSoundVolume(volume);
    }
    public void OnMusicVolumeChange()
    {
        float volume = musicSlider.value;
        musicImage.sprite = volume <= 0 ? musicSprites[0] : musicSprites[1];
        SoundManager.Instance.ChangeMusicVolume(volume);
    }
    private void PlayButtonSfx()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);
    }
    private void OnDisable()
    {
        SoundManager.Instance.SaveVolume();
    }

}

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

    private bool isPlaying => GameController.Instance.IsInState(GameState.GameStart);

    public override void Open()
    {
        base.Open();

        credit.SetActive(!isPlaying);
        button.SetActive(isPlaying);

        soundSlider.value = SoundManager.Instance.SoundVolume;
        musicSlider.value = SoundManager.Instance.MusicVolume;

        OnSoundVolumeChange();
        OnMusicVolumeChange();
    }
    public void OnRestartButtonClick()
   {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);
        GameController.Instance.RestartLevel();
   }

   public void OnContinueButtonClick()
   {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);
        GameController.Instance.SetGameState(GameState.GameStart);
   }

   public void OnBackToMainMenuClick()
   {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);

        if (isPlaying)
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
    private void OnDisable()
    {
        SoundManager.Instance.SaveVolume();
    }

}

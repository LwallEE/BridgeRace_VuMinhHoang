using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingUICanvas : UICanvas
{
    public enum RankingMode
    {
        Score = 0,
        Gold = 1,
    }
    [SerializeField] GameObject[] tabFocus;
    [SerializeField] TextMeshProUGUI[] txtTabs;
    [SerializeField] Color blue;
    private RankingMode currentMode;

    public void OnBackButtonClick()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);

        CloseDirectly();
    }
    public void OnSwitchMode(int modeIndex)
    {
        int index = (int)currentMode;
        if (modeIndex == index) return;

        tabFocus[index].SetActive(false);
        txtTabs[index].color = blue;

        currentMode = (RankingMode)modeIndex;

        tabFocus[modeIndex].SetActive(true);
        txtTabs[modeIndex].color = Color.white;

        InitData();
    }
    private void InitData()
    {

    }
}

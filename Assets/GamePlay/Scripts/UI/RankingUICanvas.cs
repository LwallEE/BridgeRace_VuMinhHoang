using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

public class RankingUICanvas : UICanvas
{
    [SerializeField] GameObject[] tabFocus;
    [SerializeField] TextMeshProUGUI[] txtTabs;
    [SerializeField] Color blue;
    [SerializeField] RankingSlot slotPrefab, playerRankSlot;
    [SerializeField] Transform contentPanel;

    private MiniPool<RankingSlot> pool = new MiniPool<RankingSlot>();
    private RankingMode currentMode;

    private UserRankingDto[] topUserRank;
    private UserRankingDto playerRank;
    private void Awake()
    {
        pool.OnInit(slotPrefab, 10, contentPanel);
    }
    public override async void Open()
    {
        base.Open();
        var rank = await NetworkClient.Instance.HttpGet<UserRankingResponse>("shop/user-items");
        if (rank.isSuccess)
        {
            playerRank = rank.senderUserRanking;
            topUserRank = rank.topUsers;
        }
        else
        {
            Debug.LogError(rank.message);
        }

        OnSwitchMode((int)currentMode);
    }

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
        pool.Collect();

        playerRankSlot.Init(playerRank, currentMode);
        for(int i = 0; i < topUserRank.Length; i++)
        {
            RankingSlot slot = ObjectPoolDictArray.Instance.GetGameObject(slotPrefab);
            slot.Init(topUserRank[i], currentMode);
        }
    }
}
public enum RankingMode
{
    Score = 0,
    Gold = 1,
}

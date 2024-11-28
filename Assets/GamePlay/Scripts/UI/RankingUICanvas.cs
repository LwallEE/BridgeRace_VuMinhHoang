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

    private UserRankingDto[] topUserRankScore, topUserRankCoin;
    private UserRankingDto playerRankScore, playerRankCoin;

    private void Awake()
    {
        pool.OnInit(slotPrefab, 10, contentPanel);
    }
    public override async void Open()
    {
        base.Open();
        var rank = await NetworkClient.Instance.HttpGet<UserRankingResponse>("ranking/point");
        if (rank.isSuccess)
        {
            topUserRankScore = rank.topUsers;
            playerRankScore = rank.senderUserRanking;
        }
        else
        {
            Debug.LogError(rank.message);
        }

        rank = await NetworkClient.Instance.HttpGet<UserRankingResponse>("ranking/coin");
        if (rank.isSuccess)
        {
            topUserRankCoin = rank.topUsers;
            playerRankCoin = rank.senderUserRanking;
        }
        else
        {
            Debug.LogError(rank.message);
        }
        currentMode = RankingMode.Gold;
        OnSwitchMode(0);
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
        switch (currentMode)
        {
            case RankingMode.Gold:
                playerRankSlot.Init(playerRankCoin, currentMode);
                for (int i = 0; i < topUserRankCoin.Length; i++)
                {
                    RankingSlot slot = pool.Spawn();

                    slot.Init(topUserRankCoin[i], currentMode);
                }
                break;
            case RankingMode.Score:
                playerRankSlot.Init(playerRankScore, currentMode);
                for (int i = 0; i < topUserRankScore.Length; i++)
                {
                    RankingSlot slot = pool.Spawn();

                    slot.Init(topUserRankScore[i], currentMode);
                }
                break;
        }

    }
}
public enum RankingMode
{
    Score = 0,
    Gold = 1,
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWaitingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberTxt;

    [SerializeField] private TextMeshProUGUI namePlayerTxt;

    [SerializeField] private TextMeshProUGUI scoreTxt;

    [SerializeField] private Image activeBg;
    [SerializeField] private Image playerIconAvatar;
    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite unReadySprite;
    public string PlayerId { get; private set; }
    public bool IsReady { get; private set; }
    public void Init(string playerName, int playerScore,int number,string playerId, Sprite iconSprite)
    {
        this.PlayerId = playerId;
        this.numberTxt.text = number.ToString();
        this.namePlayerTxt.text = playerName;
        this.scoreTxt.text = playerScore.ToString();
        this.playerIconAvatar.sprite = iconSprite;
        Ready(false);
    }

    public void Ready(bool isReady)
    {
        IsReady = isReady;
        if (isReady)
        {
            activeBg.sprite = readySprite;
        }
        else
        {
            activeBg.sprite = unReadySprite;
        }
    }

    public void UpdatePlayerNumberText(int number)
    {
        this.numberTxt.text = number.ToString();
    }
    

}

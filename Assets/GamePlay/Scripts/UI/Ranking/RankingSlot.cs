using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingSlot : MonoBehaviour
{
    [SerializeField] Image medalIcon, avatar, unitIcon;
    [SerializeField] TextMeshProUGUI txtName, txtScore, txtRank;
    [SerializeField] Sprite[] medals, units;
    [SerializeField] AvatarData avatarData;

    public void Init(UserRankingDto data, RankingMode mode)
    {
        if(data.userRank < 3)
        {
            medalIcon.gameObject.SetActive(true);
            medalIcon.sprite = medals[data.userRank];

            txtRank.gameObject.SetActive(false);
        }
        else
        {
            medalIcon.gameObject.SetActive(false);
            txtRank.gameObject.SetActive(true);
            txtRank.text = data.userRank.ToString();
        }
        int avatarType = 0;
        try
        {
            avatarType = int.Parse(data.userAvatar);
        }
        catch
        {

        }
        avatar.sprite = avatarData.GetAvatarByType((AvatarType)data.userRank);

        txtName.text = data.userName;
        txtScore.text = data.userScore.ToString();

        unitIcon.sprite = units[(int)mode];
    }
}

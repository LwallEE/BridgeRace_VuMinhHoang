using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName, txtCoin;
    [SerializeField] private Image imgAvatar;
    [SerializeField] AvatarData avatarData;

    private AvatarType avatarType;
    public void OnInit(string name, int coin, AvatarType avatarType)
    {
        txtName.text = name;
        txtCoin.text = coin.ToString();

        ChangeAvatar(avatarType);
    }
    public int GetCoin()
    {
        return int.Parse(txtCoin.text);
    }
    public void SetCoin(int coin)
    {
        txtCoin.text = coin.ToString();
    }
    public void AddCoin(int coinPlus)
    {
        int num = Mathf.Abs(coinPlus);
        int dir = num / coinPlus;
        StartCoroutine(CoinAnim(num, dir));
    }
    private IEnumerator CoinAnim(int num, int dir)
    {
        int coin = int.Parse(txtCoin.text);
        for (int i = 0; i < num; i++)
        {
            txtCoin.text = (coin + dir).ToString();
            yield return new WaitForSeconds(.1f);
        }
    }
    public void ChangeAvatar(AvatarType avatarType)
    {
        this.avatarType = avatarType;
        imgAvatar.sprite = avatarData.GetAvatarByType(avatarType);
    }
    public void CloneInfor(PlayerInfor infor)
    {
        OnInit(infor.txtName.text, int.Parse(infor.txtCoin.text), infor.avatarType);
    }
}

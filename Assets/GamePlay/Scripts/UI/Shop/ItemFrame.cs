using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : MonoBehaviour
{
    [SerializeField] Image imgBg, imgIcon;
    [SerializeField] GameObject focus;
    [SerializeField] Sprite equipped, normal;

    public int itemId, cost;
    ShopUICanvas shop;
    public void OnInit(ShopItemData data, ShopUICanvas shop)
    {
        imgBg.sprite = normal;
        focus.SetActive(false);
        InitData(data);
        this.shop = shop;
    }
    public void OnEquipped()
    {
        imgBg.sprite = equipped;
    }
    private void InitData(ShopItemData data)
    {
        itemId = data.itemId;
        cost = data.cost;
        imgIcon.sprite = data.icon;
    }
    public void Selected()
    {
        shop.OnSelectedItem(this);
    }
    public void OnSelected()
    {
        focus.SetActive(true);
    }
    public void OnUnselected()
    {
        focus.SetActive(false);
    }
}

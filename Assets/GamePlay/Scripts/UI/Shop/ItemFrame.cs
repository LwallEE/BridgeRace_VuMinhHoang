using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : MonoBehaviour
{
    [SerializeField] Image imgBg, imgIcon;
    [SerializeField] GameObject focus, locker;
    [SerializeField] Sprite equipped, normal;

    public int itemId, cost;
    public State state;
    ShopUICanvas shop;

    public enum State
    {
        Lock = 0,
        Unlock = 1,
        Equipped = 2,
    }
    public void OnInit(ShopItemData data, UserItemStatus status, ShopUICanvas shop)
    {
        imgBg.sprite = normal;
        focus.SetActive(false);

        if (status.isEquip)
        {
            OnEquipped();
        }
        else
        {
            state = status.isOwn ? State.Unlock : State.Lock;
        }

        InitData(data);
        InitState(state);

        this.cost = status.cost;

        this.shop = shop;
    }
    public void OnEquipped()
    {
        state = State.Equipped;

        locker.SetActive(false);
        imgBg.sprite = equipped;
    }
    public void OnUnequip()
    {
        state = State.Unlock;
        imgBg.sprite = normal;
        locker.SetActive(false);
    }
    public void OnBought()
    {
        state = State.Unlock;
        locker.SetActive(false);
    }
    private void InitData(ShopItemData data)
    {
        itemId = data.itemId;
        imgIcon.sprite = data.icon;
    }
    private void InitState(State state) 
    {
        this.state = state;
        locker.SetActive(state == State.Lock);
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

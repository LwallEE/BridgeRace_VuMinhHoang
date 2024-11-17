using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypeButton : MonoBehaviour
{
    [SerializeField] GameObject imgBg;
    [SerializeField] EquipmentType equipmentType;
    [SerializeField] ShopUICanvas shop;
    public void OnSelected()
    {
        imgBg.SetActive(true);
    }
    public void Selected()
    {
        shop.OnSelecetItemType(equipmentType);
    }
    public void OnUnselected()
    {
        imgBg.SetActive(false);
    }
}

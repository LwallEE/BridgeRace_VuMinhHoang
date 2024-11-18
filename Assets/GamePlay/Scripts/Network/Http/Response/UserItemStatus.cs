using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserItemStatus
{
    public int itemId;
    public int cost;
    public string image;
    public EquipmentType itemType;
    public bool isEquip, isOwn;

    public UserItemStatus()
    {
    }

    public UserItemStatus(int itemId, int cost, string image, EquipmentType itemType, bool isEquip, bool isOwn)
    {
        this.itemId = itemId;
        this.cost = cost;
        this.image = image;
        this.itemType = itemType;
        this.isEquip = isEquip;
        this.isOwn = isOwn;
    }

}

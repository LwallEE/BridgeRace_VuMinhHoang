using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData", order = 1)]
public class ShopData : ScriptableObject
{
    public List<ShopItemData> hats;
    public List<ShopItemData> pants;
    public List<ShopItemData> accessories;
    public List<ShopItemData> skins;
}

[System.Serializable]
public class ShopItemData
{
    public int itemId;
    public Sprite icon;
    public int cost;
}

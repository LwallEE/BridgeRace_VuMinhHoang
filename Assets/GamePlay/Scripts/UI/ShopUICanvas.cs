using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUICanvas : UICanvas
{
    [SerializeField] TextMeshProUGUI txtName, txtCoin;
    public void InitPlayerInfor(string name, int coin)
    {
        txtName.text = name;
        txtCoin.text = coin.ToString();
    }
    public void BackToHome()
    {
        GameController.Instance.ChangeCameraState(GameController.CameraState.Home);
        UIManager.Instance.OpenUI<HomeUICanvas>();
        CloseDirectly();
    }

    [SerializeField] ShopData data;
    [SerializeField] CharacterVisual characterVisual;
    [SerializeField] ItemFrame itemFramePrefab;
    [SerializeField] Transform contentPanel;

    [SerializeField] ItemTypeButton[] itemTypeButtons;

    MiniPool<ItemFrame> miniPool = new MiniPool<ItemFrame>();
    private EquipmentType currentType;
    private ItemFrame currentItem;
    private void Awake()
    {
        miniPool.OnInit(itemFramePrefab, 12, contentPanel);
        characterVisual = FindAnyObjectByType<CharacterVisual>();
    }
    private void Start()
    {
        itemTypeButtons[0].Selected();
    }
    public void OnSelecetItemType(EquipmentType type)
    {
        if(currentType != type)
        {
            if(currentType != EquipmentType.None)
            {
                itemTypeButtons[(int)currentType - 1].OnUnselected();
            }
            currentType = type;
            itemTypeButtons[(int)currentType - 1].OnSelected();

            currentItem = null;

            miniPool.Collect();
            InitItemFrames();
        }

    }
    private void InitItemFrames()
    {
        List<ShopItemData> itemData = new List<ShopItemData>();
        switch (currentType)
        {
            case EquipmentType.Hat:
                itemData = data.hats;
                break;
            case EquipmentType.LeftHand:
                itemData = data.accessories;
                break;
            case EquipmentType.Pant:
                itemData = data.pants;
                break;
        }
        for(int i = 0; i < itemData.Count; i++)
        {
            ItemFrame item = miniPool.Spawn();
            item.OnInit(itemData[i], this);
        }
    }
    public void OnSelectedItem(ItemFrame item)
    {
        if (item == currentItem) return;
        if(currentItem != null)
        {
            currentItem.OnUnselected();
        }
        currentItem = item;
        currentItem.OnSelected();
        characterVisual.ChangeSkin(currentType, item.itemId);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUICanvas : UICanvas
{
    [SerializeField] PlayerInfor playerInfor;
    public void InitPlayerInfor(PlayerInfor infor)
    {
        playerInfor.CloneInfor(infor);
    }
    public void BackToHome()
    {
        PlayButtonSfx();

        GameController.Instance.ChangeCameraState(GameController.CameraState.Home);
        UIManager.Instance.OpenUI<HomeUICanvas>();
        CloseDirectly();
    }

    [SerializeField] ShopData data;
    [SerializeField] CharacterVisual characterVisual;
    [SerializeField] ItemFrame itemFramePrefab;
    [SerializeField] Transform contentPanel;
    [SerializeField] GameObject[] buttons;
    [SerializeField] TextMeshProUGUI txtPrice;
    [SerializeField] GameObject panelLoading;
    [SerializeField] ItemTypeButton[] itemTypeButtons;

    MiniPool<ItemFrame> miniPool = new MiniPool<ItemFrame>();
    private EquipmentType currentType;
    private ItemFrame currentItem;
    private ItemFrame currentEquippedItem;
    private UserItemStatus[] itemStatus;
    private void Awake()
    {
        miniPool.OnInit(itemFramePrefab, 12, contentPanel);
        characterVisual = FindAnyObjectByType<CharacterVisual>();

    }
    private async void OnEnable()
    {
        var itemStatusData = await NetworkClient.Instance.HttpGet<AllItemStatusResponse>("shop/user-items");
        if (itemStatusData.isSuccess)
        {
            itemStatus = itemStatusData.items;

            itemTypeButtons[0].Selected();
        }
        else
        {
            Debug.Log(itemStatusData.message);
        }
    }
    public void OnSelecetItemType(EquipmentType type)
    {
        PlayButtonSfx();

        if (currentType != type)
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
        currentEquippedItem = null;
        for(int i = 0; i < itemData.Count; i++)
        {
            ItemFrame item = miniPool.Spawn();
            UserItemStatus status = itemStatus.FirstOrDefault(r => r.itemId == itemData[i].itemId);

            item.OnInit(itemData[i],status, this);
            if (status.isEquip)
            {
                currentEquippedItem = item;
            }
        }
    }
    public void OnSelectedItem(ItemFrame item)
    {
        PlayButtonSfx();

        if (item == currentItem) return;
        if(currentItem != null)
        {
            currentItem.OnUnselected();
        }
        currentItem = item;
        currentItem.OnSelected();
        characterVisual.ChangeSkin(currentType, item.itemId);

        txtPrice.text = currentItem.cost.ToString();
        InitButton();
    }
    private void InitButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        buttons[(int)currentItem.state].SetActive(true);
    }
    public async void OnBuy()
    {
        PlayButtonSfx();

        int coin = playerInfor.GetCoin();
        if (coin < currentItem.cost) return;

        panelLoading.SetActive(true);
        var result = await NetworkClient.Instance.HttpPost<ItemStatusResponse>("shop/buy-item",new ShopRequest(currentItem.itemId));
        if (result.isSuccess)
        {
            panelLoading.SetActive(false);

            coin -= currentItem.cost;
            playerInfor.SetCoin(coin);

            currentItem.OnBought();
            UserItemStatus status = itemStatus.FirstOrDefault(r => r.itemId == currentItem.itemId);
            status.isOwn = true;
            InitButton();
        }
    }
    public async void OnEquip()
    {
        PlayButtonSfx();

        panelLoading.SetActive(true);

        currentEquippedItem.OnUnequip();
        UserItemStatus status = itemStatus.FirstOrDefault(r => r.itemId == currentEquippedItem.itemId);
        status.isEquip = false;
        currentEquippedItem = null;


        var result = await NetworkClient.Instance.HttpPost<ItemStatusResponse>("shop/equip", new ShopRequest(currentItem.itemId));
        if (result.isSuccess)
        {
            panelLoading.SetActive(false);

            currentItem.OnEquipped();
            currentEquippedItem = currentItem;
            UserItemStatus status2 = itemStatus.FirstOrDefault(r => r.itemId == currentItem.itemId);
            status2.isEquip = true;
            InitButton();
        }
    }
    public async void OnUnequip()
    {
        PlayButtonSfx();

        panelLoading.SetActive(true);

        var result = await NetworkClient.Instance.HttpPost<ItemStatusResponse>("shop/unequip", new ShopRequest(currentItem.itemId));
        if (result.isSuccess)
        {
            panelLoading.SetActive(false);

            currentItem.OnUnequip();
            UserItemStatus status = itemStatus.FirstOrDefault(r => r.itemId == currentItem.itemId);
            status.isEquip = false;
            InitButton();
        }
    }
    private void PlayButtonSfx()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.ButtonClick);
    }
}

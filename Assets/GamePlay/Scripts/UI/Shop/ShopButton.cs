using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] Image imgButton;
    [SerializeField] GameObject goCoin;
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Sprite[] buttonSprites;
    public ItemFrame.State CurrentState { get; private set;}

    public void OnInit(ItemFrame.State state, int coin = 100)
    {
        CurrentState = state;
        imgButton.sprite = buttonSprites[(int)state];

        goCoin.SetActive(false);
        switch (state)
        {
            case ItemFrame.State.Lock:
                goCoin.SetActive(true);
                txt.text = coin.ToString();
                break;
            case ItemFrame.State.Unlock:
                txt.text = "Equip";
                break;
            case ItemFrame.State.Equipped:
                txt.text = "Unequip";
                break;
        }
    }
}

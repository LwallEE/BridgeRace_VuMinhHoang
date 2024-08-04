using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpMessageUI : UINetworkBase
{
    [SerializeField] private TextMeshProUGUI messageInfoTxt;
    [SerializeField] private Button acceptBtn;

    public void InitMessage(string message, UnityAction closeAction)
    {
        messageInfoTxt.text = message;
        acceptBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(closeAction);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        ScaleUp(0.5f);
    }

    public override void Close()
    {
        ScaleDown(0.5f);
    }
}

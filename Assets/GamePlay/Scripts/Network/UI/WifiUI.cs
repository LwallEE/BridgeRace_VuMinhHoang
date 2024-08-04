using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WifiUI : MonoBehaviour
{
    [SerializeField] private Color fastConnectionColor;

    [SerializeField] private Color averageConnectionColor;

    [SerializeField] private Color slowConnectionColor;

    [SerializeField] private Image wifiIcon;

    [SerializeField] private TextMeshProUGUI pingTxt;

    public void UpdateWifiStatus(float ping)
    {
        int pingInt = Mathf.RoundToInt(ping);
        Color color;
        if (pingInt <= 70)
        {
            color = fastConnectionColor;
        }
        else if (pingInt < 150)
        {
            color = averageConnectionColor;
        }
        else
        {
            color = slowConnectionColor;
        }

        wifiIcon.color = color;
        pingTxt.color = color;
        pingTxt.text = pingInt.ToString() + " ms";
    }
}

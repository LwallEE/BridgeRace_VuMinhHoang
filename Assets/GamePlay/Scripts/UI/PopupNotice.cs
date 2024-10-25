using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupNotice : MonoBehaviour
{
    [SerializeField] Image imgFillBg;
    [SerializeField] TextMeshProUGUI txtNotice;
    private void Start()
    {
        txtNotice.text = "";
        imgFillBg.fillAmount = 0;
    }
    public void StartNotice(string notice)
    {
        imgFillBg.DOKill();
        txtNotice.text = "";
        imgFillBg.fillAmount = 0;
        StopAllCoroutines();
        imgFillBg.DOFillAmount(1f, .2f).OnComplete(() =>
        {
            txtNotice.text = notice;
        }).SetEase(Ease.OutBack);

        StartCoroutine(CloseNotice());
    }
    private IEnumerator CloseNotice()
    {
        yield return new WaitForSeconds(2f);
        txtNotice.text = "";
        imgFillBg.DOFillAmount(0f, .5f);
    }
}

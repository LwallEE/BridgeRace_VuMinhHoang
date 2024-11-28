using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarFrame : MonoBehaviour
{
    [SerializeField] GameObject focus;
    [SerializeField] AvatarType type;
    [SerializeField] HomeUICanvas canvas;

    //private void OnEnable()
    //{
    //    focus.SetActive(false);
    //}

    public void OnSelected()
    {
        OnFocus();
        canvas.OnAvartarSelected(type);
    }
    public void OnFocus()
    {
        focus.SetActive(true);
    }
    public void OnUnselected()
    {
        focus.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarFrame : MonoBehaviour
{
    [SerializeField] GameObject focus;
    [SerializeField] AvatarType type;
    [SerializeField] HomeUICanvas canvas;

    private void OnEnable()
    {
        focus.SetActive(false);
    }

    public void OnSelected()
    {
        focus.SetActive(true);
        canvas.OnAvartarSelected(type);
    }
    public void OnUnselected()
    {
        focus.SetActive(false);
    }
}

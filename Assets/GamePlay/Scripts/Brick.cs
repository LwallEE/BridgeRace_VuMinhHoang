using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        SetColor(colorData);
    }

    public bool CanCollect(BrickColor pickerColor)
    {
        if (pickerColor == colorData.brickColorE)
        {
            gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    private void SetColor(ColorData data)
    {
        colorData = data;
        meshRenderer.material.color = data.brickColor;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;
    public bool HasCollect { get; private set; }

    public bool CanCollect(BrickColor pickerColor)
    {
        if (pickerColor == colorData.brickColorE)
        {
            gameObject.SetActive(false);
            HasCollect = true;
            return true;
        }

        return false;
    }

    private void SetColor(ColorData data)
    {
        colorData = data;
        meshRenderer.material.color = data.brickColor;
    }

    public void Init(BrickColor color, Vector3 position)
    {
        transform.position = position;
        SetColor(GameAssets.Instance.GetColorData(color));
    }

    public bool IsMatchColor(BrickColor color)
    {
        return colorData.brickColorE == color;
    }
}

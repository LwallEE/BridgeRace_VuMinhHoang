using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider collider;

    private float currentTimeToRespawn;
    public enum EBrickState
    {
        BrickStatic,
        BrickDynamic
    }

    private EBrickState brickState;
    public bool HasCollect { get; private set; }

    public bool CanCollect(BrickColor pickerColor)
    {
        if (pickerColor == colorData.brickColorE || colorData.brickColorE == BrickColor.Grey)
        {
            //collect brick
            Active(false);
            currentTimeToRespawn = Constants.TIME_TO_BRICK_RESPAWN;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (HasCollect)
        {
            if (currentTimeToRespawn > 0)
            {
                currentTimeToRespawn -= Time.deltaTime;
            }
            else
            {
                Active(true);
            }
        }
    }

    private void SetColor(ColorData data)
    {
        colorData = data;
        meshRenderer.material.color = data.brickColor;
    }

    public void InitBrickStatic(BrickColor color, Vector3 position)
    {
        transform.position = position;
        SetColor(GameAssets.Instance.GetColorData(color));
        brickState = EBrickState.BrickStatic;
        Active(true);
    }

    private void Active(bool isActive)
    {
        HasCollect = !isActive;
        meshRenderer.enabled = isActive;
        collider.enabled = isActive;
    }

    public bool IsMatchColor(BrickColor color)
    {
        return colorData.brickColorE == color;
    }
}

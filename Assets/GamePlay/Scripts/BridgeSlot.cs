using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSlot : MonoBehaviour
{
  private ColorData currentBridgeColor;
  [SerializeField] private MeshRenderer meshRenderer;

  private void Start()
  {
    SetColor(Constants.BRIDGE_INITIAL_COLOR);
  }

  public void SetColor(BrickColor color)
  {
    this.currentBridgeColor = GameAssets.Instance.GetColorData(color);
    this.meshRenderer.material.color = currentBridgeColor.brickColor;
  }

  public bool CanGoingThroughBridge(BrickColor color)
  {
    if (this.currentBridgeColor.brickColorE == BrickColor.None) return false;
    return color == this.currentBridgeColor.brickColorE;
  }

  public bool IsMatchColor(BrickColor color)
  {
    return currentBridgeColor.brickColorE == color;
  }
}

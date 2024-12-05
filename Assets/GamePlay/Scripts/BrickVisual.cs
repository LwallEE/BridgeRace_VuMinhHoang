using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickVisual : MonoBehaviour
{
   private ColorData color;
   [SerializeField] private MeshRenderer meshRenderer;

   public void UpdateVisual(ColorData color,float yPos)
   {
      this.color = color;
      transform.localRotation = Quaternion.identity;
      meshRenderer.material = this.color.brickMaterial;
      transform.localPosition = new Vector3(0, yPos, 0);
   }
}

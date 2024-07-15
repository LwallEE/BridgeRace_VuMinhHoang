using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyAssetData/CharacterData")]
public class CharacterData : ScriptableObject
{
    public float moveSpeed;

    [Header("Check Ground")] 
    public float checkGroundDistance;

    public LayerMask layerOfGrounded;
}

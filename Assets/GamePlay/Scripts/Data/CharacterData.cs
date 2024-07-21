using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyAssetData/CharacterData")]
public class CharacterData : ScriptableObject
{
    public float moveSpeed;

    [Header("Fall")]
    public float forceFall;
    [Header("Check Ground")] 
    public float checkGroundDistance;

    public LayerMask layerOfGrounded;
    [Header("Check Bridge")]
    public float checkBridgeDistance;

    public LayerMask layerOfBridge;
}

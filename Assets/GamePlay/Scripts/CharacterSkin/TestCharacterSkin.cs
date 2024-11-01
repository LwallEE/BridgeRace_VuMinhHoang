using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterSkin : MonoBehaviour
{
    [SerializeField] private int hatIndex;

    [SerializeField] private int pantIndex;

    [SerializeField] private int leftHandIndex;

    private CharacterVisual _characterVisual;

    private void Awake()
    {
        _characterVisual = GetComponent<CharacterVisual>();
    }

    [ContextMenu("Equip Skin")]
    void Equip()
    {
        _characterVisual.ChangeAllSkin(hatIndex, pantIndex, leftHandIndex);
    }

    [ContextMenu("Reset Skin")]
    void ResetToNormal()
    {
        _characterVisual.ResetSkinToNormal();
    }
}

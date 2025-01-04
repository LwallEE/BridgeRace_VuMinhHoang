using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ReuseSystem.Helper;
using ReuseSystem.Helper.Extensions;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    //Object link to skin
    [SerializeField] private SkinnedMeshRenderer pantSkinMeshRender;
    [SerializeField] private List<SkinItem> hairContainer;
    [SerializeField] private List<SkinItem> leftHandContainer;
    [SerializeField] private List<SkinItem> pantsContainer;
    [SerializeField] private List<SkinItem> wingContainer;
    [SerializeField] private SkinnedMeshRenderer bodySkinnedMeshRenderer;
    [SerializeField] private Color normalPlayerColor;
    
    //Color random for bot
    [SerializeField] private List<Color> colorToRandom;


    private Color characterColor;

    public Color GetColor()
    {
        return bodySkinnedMeshRenderer.material.color;
    }
    private void RandomFullSkin()
    {
        
    }

    public void SetColor(Color color)
    {
        bodySkinnedMeshRenderer.material.color = color;
        pantSkinMeshRender.material.color = color;

        characterColor = color;
    }

    [SerializeField]Color hitColor;
    float hitDuration = 0.1f;
    public void OnHitColor()
    {
        SoundManager.Instance.PlayShotOneTime(ESound.OnHit, Random.Range(0.1f, .8f));

        bodySkinnedMeshRenderer.material.DOColor(hitColor, hitDuration);
        bodySkinnedMeshRenderer.material.DOColor(characterColor, hitDuration).SetDelay(hitDuration);
    }
    private SkinItem GetItem(int itemId, EquipmentType type)
    {
        if (type == EquipmentType.Hat)
        {
            return hairContainer.Find(x => x.itemId == itemId);
        }

        if (type == EquipmentType.Pant)
        {
            return pantsContainer.Find(x => x.itemId == itemId);
        }

        if (type == EquipmentType.LeftHand)
        {
            return leftHandContainer.Find(x => x.itemId == itemId);
        }

        return default;
    }

    void ActiveItem(int itemId, EquipmentType type)
    {
        var item = GetItem(itemId, type);
        if(item.gameObjectItem != null)
            item.gameObjectItem.SetActive(true);
    }

    private void DisableAllItem(EquipmentType type)
    {
        if (type == EquipmentType.Hat)
        {
            hairContainer.ForEach(item => item.gameObjectItem.SetActive(false));
        }

        if (type == EquipmentType.Pant)
        {
            pantsContainer.ForEach(item => item.gameObjectItem.SetActive(false));
        }

        if (type == EquipmentType.LeftHand)
        {
            leftHandContainer.ForEach(item => item.gameObjectItem.SetActive(false));
        }
    }
  
    public void ResetSkinToNormal()
    {
        DisableAllItem(EquipmentType.Hat);
        DisableAllItem(EquipmentType.Pant);
        DisableAllItem(EquipmentType.LeftHand);
        bodySkinnedMeshRenderer.material.mainTexture = null;
        bodySkinnedMeshRenderer.material.color = normalPlayerColor;
        pantSkinMeshRender.enabled = true;
        ChangeSkin(EquipmentType.Pant, -1);
    }

    public void ChangeAllSkin(int hatId, int pantId, int leftHandId)
    {
        ChangeSkin(EquipmentType.Hat, hatId);
        ChangeSkin(EquipmentType.Pant, pantId);
        ChangeSkin(EquipmentType.LeftHand, leftHandId);
    }

    
    public void ChangeSkin(EquipmentType type, int indexSkin = 0 )
    {
        if (type == EquipmentType.Hat)
        {
            DisableAllItem(EquipmentType.Hat);
            ActiveItem(indexSkin, type);
            return;
        }

        if (type == EquipmentType.Pant)
        {
            var item = GetItem(indexSkin, type);
            var mainTexture = item.textureItem;
            pantSkinMeshRender.material.mainTexture = mainTexture;
            Color textureColor = GetColor();
            if (mainTexture != null) textureColor = Color.white;
            pantSkinMeshRender.material.color = textureColor;
            return;
        }
        
        if (type == EquipmentType.LeftHand)
        {
            DisableAllItem(EquipmentType.LeftHand);
            ActiveItem(indexSkin, type);
        }
    }
}

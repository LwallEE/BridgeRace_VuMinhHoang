using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameFollow : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private TextMeshPro nameTxt;
    private Transform targetPlayer;
    
    public void SetTarget(Transform target,string name,Color textColor)
    {
        this.targetPlayer = target;
        this.nameTxt.text = name;
        this.nameTxt.color = textColor;
    }

    private void FixedUpdate()
    {
        if (targetPlayer != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetPlayer.position + offset,
                Time.deltaTime * lerpSpeed);
        }
    }
    
}

using StateMachineNP;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagmaGround : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            MagmaEffect magmaEffect = other.GetComponent<MagmaEffect>();
            if (magmaEffect != null) return;
            other.AddComponent<MagmaEffect>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            MagmaEffect magmaEffect = other.GetComponent<MagmaEffect>();
            if (magmaEffect == null) return;
            Destroy(magmaEffect);
        }
    }
}

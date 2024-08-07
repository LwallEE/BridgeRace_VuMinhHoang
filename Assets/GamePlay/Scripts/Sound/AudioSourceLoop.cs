using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioSourceLoop : MonoBehaviour
{
    [SerializeField] private SoundSO[] data;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        ActionSource(false);
    }

    public void ActionSource(bool isStart)
    {
        if (isStart)
        {
            //Debug.Log(data[0].soundType + " loop play");
            source.clip = data[Random.Range(0, data.Length)].soundClip;
            source.Play();
            
        }
        else
        {
            source.Stop();
        }
    }

    public bool CheckIsType(ESound type)
    {
        return data[0].soundType == type;
    }
    
}

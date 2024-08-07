using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyAssetData/SoundData")]
public class SoundSO : ScriptableObject
{
    public ESound soundType;
    public AudioClip soundClip;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNP;
public class AnimationEventToCharacter : MonoBehaviour
{
    private StateMachineNP.Character character;

    private void Awake()
    {
        character = GetComponentInParent<StateMachineNP.Character>();
    }

    public void AnimationFinish()
    {
        character.AnimationFinishEvent();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            StateMachineNP.Character character = other.GetComponent< StateMachineNP.Character>();
            character.DropAllBrick();
        }
    }
}

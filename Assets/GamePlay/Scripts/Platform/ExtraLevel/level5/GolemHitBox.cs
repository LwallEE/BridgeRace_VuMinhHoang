using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHitBox : MonoBehaviour
{
    [SerializeField] PlayerHealPoint hp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            StateMachineNP.Character character = other.GetComponent< StateMachineNP.Character>();
            character.DropAllBrick();
            hp.OnHit();
        }
    }
}

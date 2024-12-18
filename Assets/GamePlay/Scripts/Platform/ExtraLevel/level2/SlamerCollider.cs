using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            var character = other.GetComponent<StateMachineNP.Character>();
            if (character != null && character.CanFall())
            {
                character.Fall(-character.GetMoveDirection());
            }
        }
    }
}

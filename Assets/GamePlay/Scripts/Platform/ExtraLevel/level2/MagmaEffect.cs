using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class MagmaEffect : MonoBehaviour
    {
        Character character;
        private float timer = 0;
        private void Start()
        {
            character = GetComponent<Character>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0f;
                character?.DropOneBrick();
            }
        }
    }
}


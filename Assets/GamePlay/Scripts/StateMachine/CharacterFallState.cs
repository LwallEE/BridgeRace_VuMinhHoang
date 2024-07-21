using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class CharacterFallState : State
    {
        private Vector3 fallDirection;

        public void SetFallDirection(Vector3 direction)
        {
            this.fallDirection = direction;
        }
        public override void Enter()
        {
            base.Enter();
            character.AddForce(fallDirection.normalized * Constants.CHARACTER_FALL_FORCE);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exit " + gameObject.name);
            character.BackFromFallToNormal();
        }
    }
}

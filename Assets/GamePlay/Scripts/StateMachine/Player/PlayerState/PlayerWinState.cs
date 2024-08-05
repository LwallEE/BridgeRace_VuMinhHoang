using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class PlayerWinState : State
    {
        private PlayerController playerController;

        public void OnInit(Character player, StateMachine stateMachine,
            CharacterData playerData, PlayerController playerController)
        {
            this.OnInit(player, stateMachine,playerData);
            this.playerController = playerController;
        }

        public override void Enter()
        {
            base.Enter();
            playerController.SetVelocityWithoutRotate(Vector3.zero);
        }
    }
}

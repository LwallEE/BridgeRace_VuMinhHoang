using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class PlayerRunState : State
    {
        private PlayerController playerController;

        public void OnInit(Character player, StateMachine stateMachine,
            CharacterData playerData, PlayerController playerController)
        {
            this.OnInit(player, stateMachine,playerData);
            this.playerController = playerController;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            HandleMove();
        }

        void HandleMove()
        {
            
            var direction = playerController.GetProjectedDirectionOnPlane(Ultility.ConvertFrom2DVectorTo3DPlane(playerController.GetMoveDirection()));
            
            if (playerController.CheckBridgeCollide())
            {
                var bridge = playerController.GetBridgeCollide();
                if (!bridge.isAllowedGoingThrough)
                {
                    playerController.UpdateRotation(direction);
                    direction.z = 0f;
                    direction.y = 0f;
                    playerController.SetVelocityWithoutRotate(direction*_entityData.moveSpeed);
                    return;
                }
            }
           
            playerController.SetVelocity(direction*_entityData.moveSpeed);
        }
    }

}

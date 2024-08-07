using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StateMachineNP
{
    public class PlayerRunState : State
    {
        private PlayerController playerController;
        public bool isTest;
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
            if (isTest)
            {
                direction = Vector3.forward;
            }
            else{
            if (playerController.CheckBridgeCollide())
            {
                var bridge = playerController.GetBridgeCollide();
                if (!bridge.CanGoingThroughBridge(playerController.GetColor()))
                {
                   
                    //try to fill the bridge with color
                    if (!playerController.HandleFillTheBridge(bridge, direction))
                    {
                        return;
                    }
                   
                }
            }
            }
            playerController.SetVelocity(direction*playerController.CurrentSpeed);
        }
    }

}

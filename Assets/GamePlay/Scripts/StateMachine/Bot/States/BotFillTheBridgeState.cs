using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class BotFillTheBridgeState : State
    {
        public enum EBotFillTheBridgeSubState
        {
            GoToTheBridge,
            GoToTheBridgeSlot,
            FailToFillTheBridge,
            GoToNextStage,
            GoToWinStage
        }
        private Bot bot;
        private Bridge bridgeTarget;
        private EBotFillTheBridgeSubState currentState;
        private BridgeSlot bridgeSlotTarget;
        private int currentBridgeIndex;
        
        public void OnInit(Character player, StateMachine stateMachine,
            CharacterData playerData, Bot bot)
        {
            this.OnInit(player, stateMachine,playerData);
            this.bot = bot;
        }

        public override void Enter()
        {
            base.Enter();
            EnterGoToTheBridgeState();
        }

        void EnterGoToTheBridgeState()
        {
           
            bridgeTarget = bot.FindOptimalBridge();
            bot.MoveToTarget(bridgeTarget.GetTheInitialPositionOfBridge());
            currentState = EBotFillTheBridgeSubState.GoToTheBridge;
        }

        void EnterGoToTheBridgeSlotState()
        {
           
            currentBridgeIndex = 0;
            bridgeSlotTarget = bridgeTarget.GetBridgeIndex(currentBridgeIndex);
            bot.MoveToTarget(bridgeSlotTarget.transform.position);
            currentState = EBotFillTheBridgeSubState.GoToTheBridgeSlot;
        }

        public bool IsInState(EBotFillTheBridgeSubState state)
        {
            return currentState == state;
        }
        void FillTheBridge()
        {
            //if the bridge slot is not the same color with bot's color
            if (!bridgeSlotTarget.CanGoingThroughBridge(bot.GetColor()))
            {
               //try to fill
               if (bot.CanFillTheBridge())
               {
                   bridgeSlotTarget.SetColor(bot.GetColor());
                   bot.RemoveBrick();
               }//if failed;
               else
               {
                   currentState = EBotFillTheBridgeSubState.FailToFillTheBridge;
                   return;
               }
            }
            //if can go through or can fill, go to the next 
            SetNextBridgeSlot();
        }

        void SetNextBridgeSlot()
        {
            //move to the next bridge slot
            currentBridgeIndex++;
            bridgeSlotTarget = bridgeTarget.GetBridgeIndex(currentBridgeIndex);
            //if not reach the final slot yet
            if (bridgeSlotTarget != null)
            {
                bot.MoveToTarget(bridgeSlotTarget.transform.position);
                currentState = EBotFillTheBridgeSubState.GoToTheBridgeSlot;
            }
            else //if reach final slot, check if is exist the next stage
            {
                if (bot.NextStage())
                {
                    currentState = EBotFillTheBridgeSubState.GoToNextStage;
                }
                else
                {
                    currentState = EBotFillTheBridgeSubState.GoToWinStage;
                }
            }
           
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (currentState == EBotFillTheBridgeSubState.GoToTheBridge)
            {
                if (bot.CheckReachTarget())
                {
                    EnterGoToTheBridgeSlotState();
                }
            }

            if (currentState == EBotFillTheBridgeSubState.GoToTheBridgeSlot)
            {
                if (bot.CheckReachTarget())
                {
                    FillTheBridge();
                }
            }
        }
    }
}


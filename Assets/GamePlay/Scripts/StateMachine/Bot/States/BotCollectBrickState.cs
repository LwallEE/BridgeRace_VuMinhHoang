using System.Collections;
using System.Collections.Generic;
using StateMachineNP;
using UnityEngine;

namespace StateMachineNP
{
    public class BotCollectBrickState : State
    {
        private Bot bot;
        
        public enum EBotCollectSubState
        {
            FindNearestBrick,
            GoToTheBrick
        }
        public bool HasFindNearestBrick { get; private set; }
        public bool HasCollectBrick { get; private set; }
        
        public EBotCollectSubState CurrentState { get; private set; }
    
        private Brick target;
        
        public void OnInit(Character player, StateMachine stateMachine,
            CharacterData playerData, Bot bot)
        {
            this.OnInit(player, stateMachine,playerData);
            this.bot = bot;
        }

        public override void Enter()
        {
            base.Enter();
            CurrentState = EBotCollectSubState.FindNearestBrick;
            HasFindNearestBrick = true;
            HasCollectBrick = false;
            FindNearestBrickWithSameColor();
        }

        void FindNearestBrickWithSameColor()
        {
            target = bot.FindNearestBrickWithColor();
            HasFindNearestBrick = target != null;
            if (HasFindNearestBrick)
            {
                //change to collect brick state
                CurrentState = EBotCollectSubState.GoToTheBrick;
                bot.MoveToTarget(target.transform);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (CurrentState == EBotCollectSubState.GoToTheBrick)
            {
               
                if (bot.CheckReachTarget())
                {
                    HasCollectBrick = true;
                }
            }
        }
    }
}

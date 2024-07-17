using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class BotWinState : State
    {
        private Bot bot;
        private const string WinAnimBoolName = "win";
        public enum EBotWinSubState
        {
            GoToWinPos,
            ReachToWinPos
        }

        private EBotWinSubState currentState;
        public void OnInit(Character player, StateMachine stateMachine,
            CharacterData playerData, Bot bt)
        {
            this.OnInit(player, stateMachine,playerData);
            this.bot = bt;
        }

        public override void Enter()
        {
            base.Enter();
            bot.MoveToTarget(bot.GetWinPos());
            currentState = EBotWinSubState.GoToWinPos;
         
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (currentState == EBotWinSubState.GoToWinPos)
            {
                if (bot.CheckReachTarget())
                {
                    EnterReachWinPos();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            bot.Anim.SetBool(WinAnimBoolName, false);
        }

        void EnterReachWinPos()
        {
            Debug.Log("Winnnnnn");
            bot.StayIdle();
            currentState = EBotWinSubState.ReachToWinPos;
            bot.Anim.SetBool(WinAnimBoolName, true);
            bot.Anim.SetBool(animBoolName, false);
        }
    }
}


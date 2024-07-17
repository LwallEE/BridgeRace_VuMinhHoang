using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class BotIdleState : State
    {
        private Bot bot;
        public void OnInit(Character player, StateMachine stateMachine,
            CharacterData playerData, Bot bot)
        {
            this.OnInit(player, stateMachine,playerData);
            this.bot = bot;
        }

        public override void Enter()
        {
            base.Enter();
            bot.StayIdle();
        }
    }
}

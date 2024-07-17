using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class FriendlyBot : Bot
    {

        #region States

        [SerializeField] private BotIdleState botIdleState;
        [SerializeField] private BotFillTheBridgeState botFillTheBridgeState;
        [SerializeField] private BotCollectBrickState botCollectBridgeState;


        #endregion

        public override void Awake()
        {
            base.Awake();
            botIdleState.OnInit(this, StateMachine, data, this);
            botFillTheBridgeState.OnInit(this, StateMachine, data, this);
            botCollectBridgeState.OnInit(this, StateMachine, data, this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(botIdleState);
        }

        protected override void OnInit()
        {
            SetColor(BrickColor.Green);
            base.OnInit();
            
        }

        public override void ChangeFromStateToState(State fromState)
        {
            base.ChangeFromStateToState(fromState);
            if (fromState == botCollectBridgeState)
            {
                if (!botCollectBridgeState.HasFindNearestBrick)
                {
                    StateMachine.ChangeState(botIdleState);
                    return;
                }

                if (botCollectBridgeState.HasCollectBrick)
                {
                    if (CheckEnoughBrickToFill())
                    {
                        StateMachine.ChangeState(botFillTheBridgeState);
                        return;
                    }
                    else
                    {
                        StateMachine.ChangeState(botCollectBridgeState);
                        return;
                    }
                }
            }

            if (fromState == botIdleState)
            {
                if (IsExistBrickWithColor())
                {
                    StateMachine.ChangeState(botCollectBridgeState);
                    return;
                }
            }

            if (fromState == botFillTheBridgeState)
            {
                if (botFillTheBridgeState.IsInState(BotFillTheBridgeState.EBotFillTheBridgeSubState
                        .FailToFillTheBridge))
                {
                    StateMachine.ChangeState(botCollectBridgeState);
                    return;
                }

                if (botFillTheBridgeState.IsInState(BotFillTheBridgeState.EBotFillTheBridgeSubState.FinishTheBridge))
                {
                    StateMachine.ChangeState(botIdleState);
                    return;
                }
            }
        }

        public override bool CheckEnoughBrickToFill()
        {
            return GetNumberOfCurrentBrick() >= currentStage.GetMinimumBridgeSlotOfBridge();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class NormalBot : Bot
    {
         #region States

        [SerializeField] private BotIdleState botIdleState;
        [SerializeField] private BotFillTheBridgeState botFillTheBridgeState;
        [SerializeField] private BotCollectBrickState botCollectBridgeState;


        #endregion

        [SerializeField] private int numberOfBrickToBuildBridge;

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
            SetColor(botColor);
            base.OnInit();
            
        }

        public override void ChangeFromStateToState(State fromState)
        {
            base.ChangeFromStateToState(fromState);
            if (!GameController.Instance.IsInState(GameState.GameStart)) return;
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
                if (CheckEnoughBrickToFill())
                {
                    StateMachine.ChangeState(botFillTheBridgeState);
                    return;
                }
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
                
                if (botFillTheBridgeState.IsInState(BotFillTheBridgeState.EBotFillTheBridgeSubState.GoToNextStage))
                {
                    StateMachine.ChangeState(botCollectBridgeState);
                    return;
                }
            }

            if (fromState == fallState)
            {
                if (fallState.IsAnimationFinish())
                {
                    //Debug.Log("Exxxit");
                    StateMachine.ChangeState(botCollectBridgeState);
                    return;
                }
            }
        }

        public override bool CheckEnoughBrickToFill()
        {
            return GetNumberOfCurrentBrick() >= numberOfBrickToBuildBridge;
        }
    }
}

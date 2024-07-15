using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StateMachineNP
{
    public class PlayerController : Character
    {
        private FloatingJoystick joyStickInput;
        
        #region States

        [SerializeField] private PlayerIdleState playerIdleState;
        [SerializeField] private PlayerRunState playerRunState;
        #endregion

        
        public override void Awake()
        {
            base.Awake();
            joyStickInput = FindObjectOfType<FloatingJoystick>();
            playerIdleState.OnInit(this, StateMachine, data, this);
            playerRunState.OnInit(this, StateMachine, data, this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(playerIdleState);
        }

        public override void ChangeFromStateToState(State fromState)
        {
            base.ChangeFromStateToState(fromState);
            if (fromState == playerIdleState)
            {
                if (GetMoveDirection() != Vector2.zero)
                {
                    StateMachine.ChangeState(playerRunState);
                    return;
                }
            }

            if (fromState == playerRunState)
            {
                if (GetMoveDirection() == Vector2.zero)
                {
                    StateMachine.ChangeState(playerIdleState);
                    return;
                }
            }
        }

        public Vector2 GetMoveDirection()
        {
            return joyStickInput.Direction;
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(checkGroundPoint.position, checkGroundPoint.position + Vector3.down*data.checkGroundDistance);
            #endif
        }
    }
}


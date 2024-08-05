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

        [SerializeField] protected PlayerIdleState playerIdleState;
        [SerializeField] protected PlayerRunState playerRunState;
        [SerializeField] protected PlayerWinState playerWinState;
        #endregion

        public bool isAutomaticMove;
        public override void Awake()
        {
            base.Awake();
            joyStickInput = FindObjectOfType<FloatingJoystick>();
            playerIdleState.OnInit(this, StateMachine, data, this);
            playerRunState.OnInit(this, StateMachine, data, this);
            playerWinState.OnInit(this, StateMachine, data, this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(playerIdleState);
            SetColor(Constants.PLAYER_BRICK_COLOR);
        }

        public override void ChangeFromStateToState(State fromState)
        {
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
                if (isAutomaticMove) return;
                if (GetMoveDirection() == Vector2.zero)
                {
                    StateMachine.ChangeState(playerIdleState);
                    return;
                }
            }

            if (fromState == fallState)
            {
                if (fallState.IsAnimationFinish())
                {
                    StateMachine.ChangeState(playerIdleState);
                    return;
                }
            }
        }

        public Vector2 GetMoveDirection()
        {
            if(joyStickInput == null)  return Vector2.zero;
            return joyStickInput.Direction;
        }
        public void SetVelocity(Vector3 velocity)
        {
            RigidbodyObj.velocity = velocity;
          
            UpdateRotation();
        }

        public void SetVelocityWithoutYAxis(Vector2 velocity)
        {
            RigidbodyObj.velocity = new Vector3(velocity.x, RigidbodyObj.velocity.y, velocity.y);
            UpdateRotation();
        }

        public void SetVelocityWithoutRotate(Vector3 velocity)
        {
            RigidbodyObj.velocity = velocity;
        }

        public void UpdateRotation(Vector3 rotation)
        {
            transform.forward = new Vector3(rotation.x, 0, rotation.z);
        }
        
        public void UpdateRotation()
        {
            transform.forward = new Vector3(RigidbodyObj.velocity.x, 0, RigidbodyObj.velocity.z);
        }
        
        
        //Get the projected direction when move on plane: slope, ground
        public Vector3 GetProjectedDirectionOnPlane(Vector3 moveDirection)
        {
            Physics.Raycast(checkGroundPoint.position, Vector3.down, out hitGround, data.checkGroundDistance,
                data.layerOfGrounded);
            if (hitGround.collider == null) return Vector3.zero;
            return Vector3.ProjectOnPlane(moveDirection, hitGround.normal).normalized;
        }


        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(checkGroundPoint.position, checkGroundPoint.position + Vector3.down*data.checkGroundDistance);
            #endif
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(Constants.CHARACTER_TAG))
            {
                Debug.Log(other.gameObject.name);
            }
        }
        
        //return true if can fill, else return false
        public virtual bool HandleFillTheBridge(BridgeSlot bridge,Vector3 direction)
        {
            if (CanFillTheBridge())
            {
                bridge.SetColor(GetColor());
                RemoveBrick();
                return true;
            }
            else
            {
                Debug.Log("not going through bridge");
                //prevent player going through bridge
                UpdateRotation(direction);
                direction.z = 0f;
                direction.y = 0f;
                SetVelocityWithoutRotate(direction*data.moveSpeed);
                return false;
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.CompareTag(Constants.WINPOS_TAG))
            {
                GameController.Instance.SetGameState(GameState.GameEndWin);
                StateMachine.ChangeState(playerWinState);
            }
        }

        public void InitPlayerReference(FloatingJoystick joystick)
        {
            this.joyStickInput = joystick;
        }
    }
}


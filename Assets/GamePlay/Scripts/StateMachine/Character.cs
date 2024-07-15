using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class Character : MonoBehaviour
    {
        public Animator Anim { get; private set; }
        public Rigidbody RigidbodyObj { get; private set; }
        public StateMachine StateMachine { get; private set; }
        
        [SerializeField] protected CharacterData data;
        [SerializeField] protected Transform checkGroundPoint;
        protected RaycastHit hitGround;
        protected RaycastHit hitBridge;
        protected BrickColor characterColor;

        protected List<Transform> brickTransform;
        public virtual void Awake()
        {
            Anim = GetComponentInChildren<Animator>();
            RigidbodyObj = GetComponent<Rigidbody>();
            StateMachine = new StateMachine();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            if(StateMachine.CurrentState != null) 
                StateMachine.CurrentState.LogicUpdate();
        }

        protected void FixedUpdate()
        {
            if(StateMachine.CurrentState != null)
                StateMachine.CurrentState.PhysicsUpdate();
        }

        public virtual void ChangeFromStateToState(State fromState)
        {
            
        }

        protected void SetColor(BrickColor color)
        {
            this.characterColor = color;
            
        }

        #region HandldBrick

        protected void AddBrick()
        {
            
        }
        

        #endregion
        //Check collide with bridge,only check if move forward
        public bool CheckBridgeCollide()
        {
            if (transform.forward.z <= 0) return false;
            return Physics.Raycast(checkGroundPoint.position, Vector3.forward, out hitBridge,
                data.checkBridgeDistance, data.layerOfBridge);
        }

        public Bridge GetBridgeCollide()
        {
            if (hitBridge.collider == null) return null;
            return hitBridge.collider.GetComponent<Bridge>();
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
    }
}


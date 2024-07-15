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
        protected Vector3 moveDirection;
        
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

        public void SetVelocity(Vector3 velocity)
        {
            RigidbodyObj.velocity = velocity;
            moveDirection = velocity;
            UpdateRotation();
        }

        public void SetVelocityWithoutYAxis(Vector2 velocity)
        {
            RigidbodyObj.velocity = new Vector3(velocity.x, RigidbodyObj.velocity.y, velocity.y);
            moveDirection = new Vector3(velocity.x, 0, velocity.y);
            UpdateRotation();
        }
        
        public void UpdateRotation()
        {
            transform.forward = new Vector3(RigidbodyObj.velocity.x, 0, RigidbodyObj.velocity.z);
        }

        public Vector3 GetProjectedDirectionOnPlane(Vector3 moveDirection)
        {
            Physics.Raycast(checkGroundPoint.position, Vector3.down, out hitGround, data.checkGroundDistance,
                data.layerOfGrounded);
            if (hitGround.collider == null) return Vector3.zero;
            return Vector3.ProjectOnPlane(moveDirection, hitGround.normal).normalized;
        }
    }
}


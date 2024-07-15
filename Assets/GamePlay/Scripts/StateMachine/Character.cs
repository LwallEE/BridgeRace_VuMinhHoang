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
        public virtual void Awake()
        {
            Anim = GetComponentInChildren<Animator>();
            RigidbodyObj = GetComponent<Rigidbody>();
            StateMachine = new StateMachine();
        }

        public virtual void ChangeFromStateToState(State fromState)
        {
            
        }

        public void SetVelocity(Vector3 velocity)
        {
            RigidbodyObj.velocity = velocity;
        }

        public void SetVelocityWithoutYAxis(Vector2 velocity)
        {
            RigidbodyObj.velocity = new Vector3(velocity.x, 0, velocity.y);
        }
    }
}


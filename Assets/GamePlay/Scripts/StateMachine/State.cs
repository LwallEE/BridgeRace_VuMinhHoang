using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNP
{
    public class State : MonoBehaviour
    {
        protected Character character;
    
        protected StateMachine stateMachine;
        protected CharacterData _entityData;
        
        protected float startTime;
    
    
        protected bool isAnimationFinish;
        // protected List<EState> ListTransitionState;
        // [SerializeField] protected EState enumState;
        [SerializeField] protected string animBoolName;
    
        public virtual void OnInit(Character player, StateMachine stateMachine,
            CharacterData playerData)
        {
            this.character = player;
            this.stateMachine = stateMachine;
            this._entityData = playerData;
           
          
        }
        public void SetAnimationName(string str)
        {
            this.animBoolName = str;
        }
    
        public virtual void Enter()
        {
            DoCheck();
            character.Anim.SetBool(animBoolName, true);
            startTime = Time.time;
            isAnimationFinish = false;
            
        }
    
        public virtual void Exit()
        {
            if(!string.IsNullOrEmpty(animBoolName))
                character.Anim.SetBool(animBoolName, false);
            isAnimationFinish = false;
        }
    
        public virtual void LogicUpdate()
        {
            character.ChangeFromStateToState(this);
        }
    
        public virtual void PhysicsUpdate()
        {
            DoCheck();
        }
    
        public bool IsAnimationFinish()
        {
            return isAnimationFinish;
        }
        public virtual void DoCheck()
        {
        }
        
        public virtual void AnimationFinishTrigger() => isAnimationFinish = true;
    
        public virtual void AnimationTrigger()
        {
        }
    }

}

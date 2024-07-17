using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachineNP
{
    public class Bot : Character
    {
        protected NavMeshAgent agent;
        protected Stage currentStage;
        protected int currentStageIndex;
        [SerializeField] protected BotWinState botWinState;
        
        public override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            botWinState.OnInit(this, StateMachine,data, this);

        }

        protected override void Start()
        {
            base.Start();
            OnInit();
        }

        protected virtual void OnInit()
        {
            agent.speed = data.moveSpeed;
            currentStageIndex = 0;
            SetStage(Map.Instance.GetStage(currentStageIndex));
        }

        public void SetStage(Stage stage)
        {
            this.currentStage = stage;
        }
        
        //return if has next stage, also set the next stage
        public bool NextStage()
        {
            currentStageIndex++;
            SetStage(Map.Instance.GetStage(currentStageIndex));
            if (currentStage == null)
            {
                ChangeToWinState();
                return false;
            }

            return true;
        }
        
        //specific bot will override this method to make different behaviour
        public virtual bool CheckEnoughBrickToFill()
        {
            return false;
        }

        public virtual void ChangeToWinState()
        {
            StateMachine.ChangeState(botWinState);
        }
        public Brick FindNearestBrickWithColor()
        {
            if (currentStage != null)
                return currentStage.GetNearestBrickOfColorCanCollect(characterColor.brickColorE, transform.position);
            return null;
        }

        public bool IsExistBrickWithColor()
        {
            return FindNearestBrickWithColor() != null;
        }

        public Bridge FindOptimalBridge()
        {
            if (currentStage != null)
                return currentStage.GetOptimalBridge(characterColor.brickColorE);
            return null;
        }

        public void MoveToTarget(Transform target)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }

        public void MoveToTarget(Vector3 position)
        {
            agent.isStopped = false;
            agent.SetDestination(position);
        }

        public bool CheckReachTarget()
        {
//            Debug.Log(agent.remainingDistance);
            return Ultility.CheckEqualFloat(agent.remainingDistance, 0f,0.1f);
        }

        public Vector3 GetWinPos()
        {
            return Map.Instance.GetWinPosition();
        }

        public void StayIdle()
        {
            agent.isStopped = true;
        }
    }
}

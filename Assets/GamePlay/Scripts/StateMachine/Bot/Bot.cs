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
        public override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();

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

        public virtual bool CheckEnoughBrickToFill()
        {
            return false;
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

        public void StayIdle()
        {
            agent.isStopped = true;
        }
    }
}

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
        [SerializeField] protected BrickColor botColor;
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

        protected override void Update()
        {
            if (!GameController.Instance.IsInState(GameState.GameStart)) return;
            base.Update();
        }

        protected override void FixedUpdate()
        {
            if (!GameController.Instance.IsInState(GameState.GameStart)) return;
            base.FixedUpdate();
        }

        protected virtual void OnInit()
        {
            agent.speed = data.moveSpeed;
            BackFromFallToNormal();
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
            if (!agent.isOnNavMesh) return;
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }

        public void MoveToTarget(Vector3 position)
        {
            if (!agent.isOnNavMesh) return;
            agent.isStopped = false;
            agent.SetDestination(position);
        }

        public bool CheckReachTarget()
        {
//            Debug.Log(agent.remainingDistance);
            if (!agent.isOnNavMesh) return false;
            return Ultility.CheckEqualFloat(agent.remainingDistance, 0f,0.1f);
        }
    
        public Vector3 GetWinPos()
        {
            return Map.Instance.GetWinPosition();
        }

        public void StayIdle()
        {
            if (!agent.isOnNavMesh) return;
            agent.isStopped = true;
        }

        public override void Fall(Vector3 fallDirection)
        {
            agent.enabled = false;
            RigidbodyObj.isKinematic = false;
            base.Fall(fallDirection);
        }

        public override void AddForce(Vector3 direction)
        {
            base.AddForce(direction*Constants.BOT_FALL_FORCE_MULTIPLER);
        }

        public override void BackFromFallToNormal()
        {
            base.BackFromFallToNormal();
            agent.enabled = true;
            RigidbodyObj.isKinematic = true;
        }
    }
}

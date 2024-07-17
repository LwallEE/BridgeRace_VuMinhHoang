using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private Transform brickContainer;
        
        protected RaycastHit hitGround;
        protected RaycastHit hitBridge;
        protected ColorData characterColor;

        protected List<BrickVisual> brickList;
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

        public BrickColor GetColor()
        {
            return characterColor.brickColorE;
        }
        protected void SetColor(BrickColor color)
        {
            this.characterColor = GameAssets.Instance.GetColorData(color);
            this.meshRenderer.material.color = characterColor.characterColor ;
        }

        #region HandldBrick

        protected void AddBrick()
        {
            var brickVisual = LazyPool.Instance.GetObj<BrickVisual>(GameAssets.Instance.brickVisual);
            brickVisual.transform.SetParent(brickContainer);
            if (brickList == null)
            {
                brickList = new List<BrickVisual>();
            }
            brickList.Add(brickVisual);
            UpdateBrickVisual();
        }

        protected void UpdateBrickVisual()
        {
            float yPos = 0f;
            foreach (var brick in brickList)
            {
               brick.UpdateVisual(characterColor, yPos);
               yPos += Constants.DISTANCE_BETWEEN_BRICK_CONSTANT;
            }
        }

        public void RemoveBrick()
        {
            if (brickList.Count > 0)
            {
                LazyPool.Instance.AddObjectToPool(brickList[brickList.Count-1].gameObject);
                brickList.RemoveAt(brickList.Count-1);
            }
        }

        public int GetNumberOfCurrentBrick()
        {
            return brickList?.Count ?? 0;
        }

        #endregion

        #region HandleBridge

        //Check collide with bridge,only check if move forward
        public bool CheckBridgeCollide()
        {
            if (transform.forward.z <= 0) return false;
            return Physics.Raycast(checkGroundPoint.position, Vector3.forward, out hitBridge,
                data.checkBridgeDistance, data.layerOfBridge);
        }

        public BridgeSlot GetBridgeCollide()
        {
            if (hitBridge.collider == null) return null;
            return hitBridge.collider.GetComponent<BridgeSlot>();
        }

        public bool CanFillTheBridge()
        {
            return brickList != null && brickList.Count > 0;
        }
        
        #endregion
        
      
        protected virtual void OnTriggerEnter(Collider other)
        {
//            Debug.Log(other.name);
            if (other.CompareTag(Constants.BRICK_TAG))
            {
                var brick = other.GetComponent<Brick>();
                if (brick.CanCollect(characterColor.brickColorE))
                {
                    AddBrick();
                }
            }
        }
    }
}


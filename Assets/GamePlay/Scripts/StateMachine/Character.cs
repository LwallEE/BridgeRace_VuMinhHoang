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
            var brickVisual = Instantiate(GameAssets.Instance.brickVisual, brickContainer).GetComponent<BrickVisual>();
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
                Destroy(brickList[brickList.Count-1].gameObject);
                brickList.RemoveAt(brickList.Count-1);
            }
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

        public Bridge GetBridgeCollide()
        {
            if (hitBridge.collider == null) return null;
            return hitBridge.collider.GetComponent<Bridge>();
        }

        public bool CanFillTheBridge()
        {
            return brickList != null && brickList.Count > 0;
        }
        
        #endregion
        
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

        protected void OnTriggerEnter(Collider other)
        {
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


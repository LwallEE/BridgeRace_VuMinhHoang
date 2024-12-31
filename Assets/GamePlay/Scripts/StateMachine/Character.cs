using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace StateMachineNP
{
    public class Character : MonoBehaviour
    {
        public Animator Anim { get; private set; }
        public Rigidbody RigidbodyObj { get; private set; }
        public StateMachine StateMachine { get; private set; }

        [SerializeField] protected CharacterVisual characterVisual;
        [SerializeField] protected CharacterData data;
        [SerializeField] protected Transform checkGroundPoint;
        [SerializeField] private Transform brickContainer;
        
        //Fall State
        [SerializeField] protected CharacterFallState fallState;
        
        protected RaycastHit hitGround;
        protected RaycastHit hitBridge;
        protected ColorData characterColor;

        protected List<BrickVisual> brickList;

        protected string previousAnimName;
        public virtual void Awake()
        {
            Anim = GetComponentInChildren<Animator>();
            RigidbodyObj = GetComponent<Rigidbody>();
            StateMachine = new StateMachine();
            fallState.OnInit(this, StateMachine, data);
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            if(StateMachine.CurrentState != null) 
                StateMachine.CurrentState.LogicUpdate();
        }

        protected virtual void FixedUpdate()
        {
            if(StateMachine.CurrentState != null)
                StateMachine.CurrentState.PhysicsUpdate();
        }

        public virtual void ChangeFromStateToState(State fromState)
        {
            
        }

        public virtual void PlayAnimation(string animName, bool value)
        {
            if (animName.CompareTo("fall") == 0 && value)
            {
                SoundManager.Instance.PlayShotOneTime(ESound.PlayerFall);
                ParticleManager.Instance.PlayFxCollide(transform.position);
            }
            Anim.SetBool(animName, value);
            if (value)
            {
                this.previousAnimName = animName;
            }
        }

        public BrickColor GetColor()
        {
            return characterColor.brickColorE;
        }
        protected void SetColor(BrickColor color)
        {
            this.characterColor = GameAssets.Instance.GetColorData(color);
            characterVisual.SetColor(this.characterColor.characterColor);
        }

        public void EquipSkin(int hatId, int pantId, int leftHandId)
        {
            characterVisual.ChangeAllSkin(hatId, pantId, leftHandId);
        }

        #region HandldBrick

        protected void AddBrick()
        {
            SoundManager.Instance.PlayShotOneTime(ESound.CollectBrick);
            var brickVisual = LazyPool.Instance.GetObj<BrickVisual>(GameAssets.Instance.brickVisual);
            brickVisual.transform.SetParent(brickContainer);
            if (brickList == null)
            {
                brickList = new List<BrickVisual>();
            }
            brickList.Add(brickVisual);
            
        }

        protected void UpdateBrickVisual()
        {
            float yPos = 0f;
            if (brickList == null) return;
            foreach (var brick in brickList)
            {
               brick.UpdateVisual(characterColor, yPos);
               yPos += Constants.DISTANCE_BETWEEN_BRICK_CONSTANT;
            }
        }

        public void RemoveBrick()
        {
            if (brickList != null && brickList.Count > 0)
            {
                LazyPool.Instance.AddObjectToPool(brickList[brickList.Count-1].gameObject);
                brickList.RemoveAt(brickList.Count-1);
            }
        }

        public int GetNumberOfCurrentBrick()
        {
            return brickList?.Count ?? 0;
        }

        private void RemoveAllBrick()
        {
            if (brickList == null) return;
            foreach (var item in brickList)
            {
                LazyPool.Instance.AddObjectToPool(item.gameObject);
            }
            brickList.Clear();
            UpdateBrickVisual();
        }
        public void DropOneBrick()
        {
            if(brickList.Count < 1) return;

            characterVisual.OnHitColor();

            var brick = LazyPool.Instance.GetObj<Brick>(GameAssets.Instance.brickPrefab);
            brick.InitBrickDynamic(brickList[brickList.Count - 1].transform.position);

            RemoveBrick();
        }
        public void DropAllBrick()
        {
            characterVisual.OnHitColor();

            RemoveAllBrick();

            fallState.SetFallDirection(GetMoveDirection());
            StateMachine.ChangeState(fallState);
        }
        public void DeleteOneBrick()
        {
            characterVisual.OnHitColor();

            if (brickList.Count < 1) return;
            RemoveBrick();
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

        public virtual void AddForce(Vector3 direction)
        {
//            Debug.Log(gameObject.name + " " +direction);
            direction.y = Constants.CHARACTER_FALL_HEIGHT_FORCE;
            RigidbodyObj.AddForce(direction,ForceMode.Impulse);
        }
        public Vector3 GetMoveDirection()
        {
            return transform.forward;
        }
        public virtual void Fall(Vector3 fallDirection)
        {
            characterVisual.OnHitColor();

            FallAllBrick();
            //fallDirection.y = 0f;
            fallState.SetFallDirection(fallDirection);
            StateMachine.ChangeState(fallState);
            
        }

        private void FallAllBrick()
        {
            if (brickList == null) return;
            foreach (var item in brickList)
            {
                var brick = LazyPool.Instance.GetObj<Brick>(GameAssets.Instance.brickPrefab);
                brick.InitBrickDynamic(item.transform.position);
            }
            RemoveAllBrick();
        }

        public void AnimationFinishEvent()
        {
            if(StateMachine.CurrentState != null)
                StateMachine.CurrentState.AnimationFinishTrigger();
        }

        public bool CanFall()
        {
            return StateMachine.CurrentState != fallState;
        }

        public virtual void BackFromFallToNormal()
        {
            
        }
      
        protected virtual void OnTriggerEnter(Collider other)
        {
//            Debug.Log(other.name);
            if (other.CompareTag(Constants.BRICK_TAG))
            {
                var brick = other.GetComponent<Brick>();
                if (brick.CanCollect(characterColor.brickColorE))
                {
                    brick.CollectBrick();
                    AddBrick();
                    UpdateBrickVisual();
                }
            }

            //Kick
            if (other.CompareTag(Constants.CHARACTER_TAG))
            {
                var character = other.GetComponent<StateMachineNP.Character>();
                if (character != null && character.CanFall() &&  GetNumberOfCurrentBrick() > character.GetNumberOfCurrentBrick())
                {
                    Debug.Log(gameObject.name + " Kick " + character.gameObject.name );
                    character.Fall(GetMoveDirection());
                }
            }
        }
    }
}


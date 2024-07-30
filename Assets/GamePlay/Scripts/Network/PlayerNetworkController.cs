using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MyGame.Schema;
using StateMachineNP;
using UnityEngine;

public class PlayerNetworkController : PlayerController,IDispose
{
    [SerializeField] private BoxCollider boxCollider;
    private NetworkEventHandler networkEventHandler = new NetworkEventHandler();
    public bool IsMine { get; private set; }
    public bool IsFall { get; private set; }
    public string Id { get; private set; }
    private Vector3 destinationPosition;
    private float yRotationDestination;
    private float lerpSpeed;
    
    protected override void Start()
    {
        StateMachine.Initialize(playerIdleState);
        lerpSpeed = GameNetworkManager.Instance.LerpPlayerSpeed;
    }

    protected override void Update()
    {
        if (!GameNetworkManager.Instance.IsInGameState(GameNetworkStateEnum.GameLoop)) return;
        if (IsMine)
        {
            base.Update();
        }
        
    }

    protected override void FixedUpdate()
    {
        if (!GameNetworkManager.Instance.IsInGameState(GameNetworkStateEnum.GameLoop)) return;
        if (IsMine) 
        {
            base.FixedUpdate();
        }
        else //if not is mine, update rotation and position from server
        {
            transform.position = Vector3.Lerp(transform.position, destinationPosition,
                Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(transform.eulerAngles.x,yRotationDestination,transform.eulerAngles.z),Time.deltaTime*lerpSpeed );
            //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Lerp(transform.eulerAngles.y, yRotationDestination,Time.deltaTime * lerpSpeed), transform.eulerAngles.z);
        }
    }

    public override bool HandleFillTheBridge(BridgeSlot bridge, Vector3 direction)
    {
        if (CanFillTheBridge() && IsMine)
        {
            string id = (bridge as BridgeSlotNetwork).Id;
            if (id != null)
            {
                GameNetworkManager.Instance.RequestBuildTheBridge(id);
            }
        }
        
        UpdateRotation(direction);
        direction.z = 0f;
        direction.y = 0f;
        SetVelocityWithoutRotate(direction*data.moveSpeed);
        return false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!IsMine) return;
        if (other.CompareTag(Constants.BRICK_TAG))
        {
            var brick = other.GetComponent<BrickNetwork>();
            if(brick.CanCollect(characterColor.brickColorE))
                GameNetworkManager.Instance.RequestCollectBrick(brick.Id);
        }

        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            var otherPlayer = other.GetComponent<PlayerNetworkController>();
            if (otherPlayer != null && !otherPlayer.IsFall &&
                GetNumberOfCurrentBrick() > otherPlayer.GetNumberOfCurrentBrick())
            {
                //send message to kick other player
                GameNetworkManager.Instance.RequestKickTheOtherPlayer(GetMoveDirection(), otherPlayer.Id,otherPlayer.transform.position);
            }
        }

        if (other.CompareTag(Constants.WINPOS_TAG))
        {
            GameNetworkManager.Instance.RequestCheckPlayerWin();
        }
    }

    public override void PlayAnimation(string animName, bool value)
    {
        if (IsMine && value && animName != previousAnimName)
        {
            GameNetworkManager.Instance.UpdateAnimationOfPlayerToServer(animName);
        }
        base.PlayAnimation(animName, value);
    }

    public void InitPlayerNetwork(PlayerData data, bool isMine)
    {
        this.IsMine = isMine;
        transform.position = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.position);
        SetColor((BrickColor)data.color);
        this.destinationPosition = transform.position;
        Id = data.entityId;
        IsFall = data.isFall;
        boxCollider.center = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.centerPosition);
        boxCollider.size = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.size);
        if (!isMine)
        {
            RigidbodyObj.useGravity = false;
        }
        
        networkEventHandler.InitEventRegister(RegisterEventSync(data));;
        GameNetworkManager.Instance.AddToDisposeList(this);
        
            
    }

    private List<Action> RegisterEventSync(PlayerData player)
    {
        List<Action> returnActions = new List<Action>();
        if (!IsMine)
        {
            returnActions.Add(player.OnPositionChange(delegate(Vect3 value, Vect3 previousValue)
            {
//                Debug.Log("position change " + NetworkUltilityHelper.ConvertFromVect3ToVector3(value));
                SetDestination(value);
            }));
            returnActions.Add(player.OnYRotationChange((value, previousValue) =>
            {
                SetYRotation(value);
            }));
            returnActions.Add( player.OnAnimNameChange((value, previousValue) =>
            {
                //Debug.Log("animName change " + value + " "+previousValue);
                SetAnimName(value, previousValue);
            }));

        }
      
        returnActions.Add(player.OnNumberOfBrickChange((value, previousValue) =>
        {
//            Debug.Log("change brick from " + previousValue + " to " + value );
            int changeBrickNum = value - previousValue;
            if (changeBrickNum != 0)
            {
                UpdateBrickNumber(changeBrickNum);
            }
        }));
        returnActions.Add(player.OnIsFallChange((value, previousValue) =>
        {
            IsFall = value;
        } ));
        return returnActions;
    }
    

    #region Network Event CallBack
    private void UpdateBrickNumber(int number)
    {
        bool isRemove = number < 0;
        for (int i = 0; i < Mathf.Abs(number); i++)
        {
            if (isRemove)
            {
                RemoveBrick();
            }
            else
            {
                AddBrick();
            }
        }
        UpdateBrickVisual();
    }

    public void SetDestination(Vect3 destination)
    {
        this.destinationPosition = NetworkUltilityHelper.ConvertFromVect3ToVector3(destination);
        //Debug.Log(this.destinationPosition);
    }

    public void SetYRotation(float yRotation)
    {
        this.yRotationDestination = yRotation;
    }

    public void SetAnimName(string newValue, string previousValue)
    {
        if(!string.IsNullOrEmpty(newValue))
            Anim.SetBool(newValue, true);
        if(!string.IsNullOrEmpty(previousValue))
            Anim.SetBool(previousValue, false);
    }

    public override void Fall(Vector3 fallDirection)
    {
        fallState.SetFallDirection(fallDirection);
        StateMachine.ChangeState(fallState);
    }

    #endregion
   
    public void Dispose()
    {
        networkEventHandler.UnRegister();
    }
}

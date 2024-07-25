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
        if (IsMine)
        {
            base.Update();
        }
        
    }

    protected override void FixedUpdate()
    {
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

    protected override void OnTriggerEnter(Collider other)
    {
        if (!IsMine) return;
        if (other.CompareTag("Brick"))
        {
            var brick = other.GetComponent<BrickNetwork>();
            GameNetworkManager.Instance.RequestCollectBrick(brick.Id);
        }
    }

    public override void PlayAnimation(string animName, bool value)
    {
        if (value && animName != previousAnimName)
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
        boxCollider.center = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.centerPosition);
        boxCollider.size = NetworkUltilityHelper.ConvertFromVect3ToVector3(data.boxCollider.size);
        if (!isMine)
        {
            networkEventHandler.InitEventRegister(RegisterEventSync(data));;
            GameNetworkManager.Instance.AddToDisposeList(this);
        }
            
    }

    private List<Action> RegisterEventSync(PlayerData player)
    {
        List<Action> returnActions = new List<Action>();
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
        return returnActions;
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

    public void Dispose()
    {
        networkEventHandler.UnRegister();
    }
}

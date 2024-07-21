using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Brick : MonoBehaviour
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider collider;

    private Rigidbody m_rigibody;
    private float currentTimeToRespawn;
    private bool canCollect;
    private float currentTimeCoolDownToCollect;
    public enum EBrickState
    {
        BrickStatic,
        BrickDynamic
    }

    private void Awake()
    {
        m_rigibody = GetComponent<Rigidbody>();
    }

    private EBrickState brickState;
    public bool HasCollect { get; private set; }

    public bool CanCollect(BrickColor pickerColor)
    {
        if (brickState == EBrickState.BrickDynamic && !canCollect) return false;
        if (pickerColor == colorData.brickColorE || colorData.brickColorE == BrickColor.Grey)
        {
            //collect brick
            Active(false);
            currentTimeToRespawn = Constants.TIME_TO_BRICK_RESPAWN;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (HasCollect && brickState == EBrickState.BrickStatic)
        {
            if (currentTimeToRespawn > 0)
            {
                currentTimeToRespawn -= Time.deltaTime;
            }
            else
            {
                Active(true);
            }
        }

        if (brickState == EBrickState.BrickDynamic && !canCollect)
        {
            if (currentTimeCoolDownToCollect > 0)
            {
                currentTimeCoolDownToCollect -= Time.deltaTime;
            }
            else
            {
                canCollect = true;
            }
        }
    }

    private void SetColor(ColorData data)
    {
        colorData = data;
        meshRenderer.material.color = data.brickColor;
    }

    public void InitBrickStatic(BrickColor color, Vector3 position)
    {
        transform.position = position;
        ActiveRigibody(false);
        SetColor(GameAssets.Instance.GetColorData(color));
        brickState = EBrickState.BrickStatic;
        Active(true);
    }

    public void InitBrickDynamic(Vector3 position)
    {
        transform.position = position;
        brickState = EBrickState.BrickDynamic;
        canCollect = false;
        Vector3 moveDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        ActiveRigibody(true);
        Active(true);
        m_rigibody.AddForce(moveDirection * Constants.BRICK_MOVE_FORCE,ForceMode.Impulse);
        SetColor(GameAssets.Instance.GetColorData(BrickColor.Grey));
        currentTimeCoolDownToCollect = Constants.BRICK_COOLDOWN_TIME_TO_COLLECT;

    }

    private void ActiveRigibody(bool isActive)
    {
        m_rigibody.isKinematic = !isActive;
    }
    private void Active(bool isActive)
    {
        HasCollect = !isActive;
        meshRenderer.enabled = isActive;
        collider.enabled = isActive;
    }

    public bool IsMatchColor(BrickColor color)
    {
        return colorData.brickColorE == color || colorData.brickColorE == BrickColor.Grey;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider collider;

    public void UpdateCollider(int numberOfBridge)
    {
        transform.localPosition = new Vector3(0, numberOfBridge * 1f / 2 -0.5f, numberOfBridge * 1f / 2 -0.5f);
        collider.size = new Vector3(collider.size.x, collider.size.y, numberOfBridge * 1.414f);
    }
}

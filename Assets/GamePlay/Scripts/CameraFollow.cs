using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform target;

    private void FixedUpdate()
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}

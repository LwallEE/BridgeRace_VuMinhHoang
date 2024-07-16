using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeshNavMesh : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private NavMeshAgent agent;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        agent.SetDestination(target.transform.position);
    }
}

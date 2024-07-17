using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }
    [SerializeField] private List<Stage> stages;
    private void Awake()
    {
        Instance = this;
    }

    public Stage GetStage(int index)
    {
        if (index >= 0 && index < stages.Count)
        {
            return stages[index];
        }

        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class NetworkEventHandler
{
    private List<Action> eventRegister = new List<Action>();

    public void InitEventRegister(List<Action> actions)
    {
        foreach (var action in actions)
        {
            eventRegister.Add(action);
        }
    }

    public void UnRegister()
    {
        foreach (var action in eventRegister)
        {
            action?.Invoke();
        }
        eventRegister.Clear();
    }
}

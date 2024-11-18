using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AllItemStatusResponse : GeneralResponse
{
    public UserItemStatus[] items;

    public AllItemStatusResponse(bool isSuccess, string message) : base(isSuccess, message)
    {
    }

    public AllItemStatusResponse() : base()
    { }
}

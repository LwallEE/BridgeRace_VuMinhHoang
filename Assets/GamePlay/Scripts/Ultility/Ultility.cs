using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ultility 
{
    public static Vector3 ConvertFrom2DVectorTo3DPlane(Vector2 direction)
    {
        return new Vector3(direction.x, 0, direction.y);
    }
}

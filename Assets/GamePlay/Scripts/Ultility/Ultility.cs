using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public static class Ultility 
{
    public static Vector3 ConvertFrom2DVectorTo3DPlane(Vector2 direction)
    {
        return new Vector3(direction.x, 0, direction.y);
    }

    public static Vector3 SnapVector(Vector3 vector)
    {
        return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    }

    public static bool CheckEqualFloat(float a, float b,float threshold)
    {
        return Math.Abs(a - b) <= threshold;
    }

    
   
}

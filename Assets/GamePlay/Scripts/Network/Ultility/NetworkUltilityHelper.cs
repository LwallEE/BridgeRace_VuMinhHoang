using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public static class NetworkUltilityHelper
{
    public static Vector3 ConvertFromVect3ToVector3(Vect3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    public static Vect3 ConvertFromVector3ToVect3(Vector3 vector)
    {
        return new Vect3()
        {
            x = vector.x,
            y = vector.y,
            z = vector.z
        };
    }
}

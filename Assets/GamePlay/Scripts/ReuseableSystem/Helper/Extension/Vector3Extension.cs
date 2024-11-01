using UnityEngine;

namespace ReuseSystem.Helper.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 QuadraticLerpLockYAxis(Vector3 from, Vector3 to, float speed,Vector3 P1)
        {
            speed = Mathf.Clamp(speed, 0, 1);
            return (1 - speed) * (1 - speed) * from + 2 * (1 - speed) * speed * P1 + speed * speed * to;
        }
    }
}
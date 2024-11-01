using UnityEngine;

namespace ReuseSystem.Helper.Extensions
{
    public static class TransformExtension
    {
        public static void DisableAllChild(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
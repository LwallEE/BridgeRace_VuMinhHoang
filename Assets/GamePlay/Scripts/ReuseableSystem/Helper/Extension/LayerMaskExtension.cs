using UnityEngine;

namespace ReuseSystem.Helper.Extensions
{
    public static class LayerMaskExtension
    {
        public static bool IsInLayerMask(this LayerMask layer, int otherLayerValue)
        {
            return ((1 << otherLayerValue) & layer) != 0;
        }
    }
}
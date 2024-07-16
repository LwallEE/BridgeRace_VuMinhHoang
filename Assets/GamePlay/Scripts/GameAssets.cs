using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : Singleton<GameAssets>
{
    public GameObject brickVisual;

    [Header("Material")]

    #region Material

    public List<ColorData> colorData;
    #endregion

    public ColorData GetColorData(BrickColor color)
    {
        foreach (var item in colorData)
        {
            if (item.brickColorE == color)
            {
                return item;
            }
        }

        return null;
    }
}

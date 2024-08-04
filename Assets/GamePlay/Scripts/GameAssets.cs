using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : Singleton<GameAssets>
{
    public GameObject brickVisual;
    public GameObject brickPrefab;
    [Header("Material")]

    #region Material

    public List<ColorData> colorData;
    #endregion

    [Header("Sprite")]
    public List<Sprite> roomAvatarBgSprites;
    public List<Sprite> roomAvatarIconSprites;
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

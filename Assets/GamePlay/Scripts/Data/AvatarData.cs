using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AvatarData")]
public class AvatarData : ScriptableObject
{
    public Sprite[] avatar;

    // con m* thằnng code backend dở hơi đi lưu avatar bằng string name
    public Sprite GetAvatarByType(AvatarType avatarType)
    {
        return avatar[(int) avatarType];
    }
}
public enum AvatarType
{
    Default = 0,
    Ninja = 1,
    Catus = 2,
    Chemical = 3,
    Robo = 4,
    FireMan = 5,
    WineBarrel = 6,
    RocknRoll = 7,
    Knight = 8,
    Samurai = 9,
}

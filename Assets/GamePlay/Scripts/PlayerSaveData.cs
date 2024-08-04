using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSaveData
{
    public static readonly string USER_ACHIEMENT_STR = "USER_ACHIEVEMENT";
    public static int CurrentAchievement
    {
        get => PlayerPrefs.GetInt(USER_ACHIEMENT_STR, 0);
        set => PlayerPrefs.SetInt(USER_ACHIEMENT_STR, value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const float DISTANCE_BETWEEN_BRICK_CONSTANT = 0.4f;
    public const BrickColor PLAYER_BRICK_COLOR = BrickColor.Blue;
    public const BrickColor BRIDGE_INITIAL_COLOR = BrickColor.None;
    public const string BRICK_TAG = "Brick";
    public const string CHARACTER_TAG = "Character";
    public const string WINPOS_TAG = "WinPos";
    public const float TIME_TO_BRICK_RESPAWN = 5f;
    public const float CHARACTER_FALL_FORCE = 3f;
    public const float BOT_FALL_FORCE_MULTIPLER = 3f;
    public const float CHARACTER_FALL_HEIGHT_FORCE = 5f;
    public const float BRICK_MOVE_FORCE = 3f;
    public const float BRICK_COOLDOWN_TIME_TO_COLLECT = 0.5f;
}

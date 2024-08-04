using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Schema;
using UnityEngine;

public class PlayerInputMessage
{
    public Vect3 position;
    public float yRotation;
}

public class GreyBrickPositionMessage
{
    public List<BrickPositionMessage> brickChanges;
}

[Serializable]
public class BrickPositionMessage
{
    public string key;
    public Vect3 position;
}

[Serializable]
public class ResultGameResponse
{
    public string winUserId;
    public bool isWin;
    public int scoreBonusResult;
}

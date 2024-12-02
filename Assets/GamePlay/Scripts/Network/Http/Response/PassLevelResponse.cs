using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassLevelResponse : GeneralResponse
{
    public int coinGet;
    public int nextLevel;

    public PassLevelResponse()
    {
    }

    public PassLevelResponse(int coinGet, int nextLevel)
    {
        this.coinGet = coinGet;
        this.nextLevel = nextLevel;
    }
}

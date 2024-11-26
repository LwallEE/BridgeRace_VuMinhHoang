using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserRankingDto
{
    public string userName;
    public int userRank;
    public int userScore;
    public string userAvatar;

    public UserRankingDto()
    {
    }

    public UserRankingDto(string userName, int userRank, int userScore, string userAvatar)
    {
        this.userName = userName;
        this.userRank = userRank;
        this.userScore = userScore;
        this.userAvatar = userAvatar;
    }
}

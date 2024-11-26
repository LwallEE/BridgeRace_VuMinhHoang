using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserRankingResponse : GeneralResponse
{
    public UserRankingDto[] topUsers;
    public UserRankingDto senderUserRanking;
}

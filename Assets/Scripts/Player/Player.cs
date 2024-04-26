using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : IReward
{
    public PlayerResource playerResource;
    public PlayerStat playerStat;
    public PlayerHistory playerHistory;
    //public Pyosa gukju;

    public void CancelCharge(Reward reward)
    {
        //playerResource.CancelCharge();
        //    playerStat.CancelCharge();
    }

    public void GetReward(Reward reward)
    {
        playerResource.GetReward(reward);
        playerStat.GetReward(reward);
    }
}

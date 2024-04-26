using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Reward : IReward
{
    public PlayerResource playerResource;
    public PlayerStat playerStat;

    public Reward(PlayerResource playerResource, PlayerStat playerStat)
    {
        this.playerResource = playerResource;
        this.playerStat = playerStat;
    }

    public void CancelCharge(Reward reward)
    {
        //Debug.Log($"{health} ü�� ������, {mana} ���� ������");
    }

    public void GetReward(Reward reward)
    {
        PlayerManager playerManager = Manager.ins.playerManager;

        playerManager.player.GetReward(reward);
    }

    
}

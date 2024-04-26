using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStat : IReward
{
    public int health;
    public int mana;
    public int stamina;

    public void CancelCharge()
    {
    }

    public void CancelCharge(Reward reward)
    {
        //Debug.Log($"{health} ü�� ������, {mana} ���� ������");
    }

    public void GetReward(Reward reward)
    {
        PlayerStat stat = reward.playerStat;
        health += stat.health;
        mana += stat.mana;
        stamina += stat.stamina;
        Debug.Log($"{health} ü�� ȹ��, {mana} ���� ȹ��, {stamina} ���׹̳� ȹ��");
    }
}

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
        //Debug.Log($"{health} Ã¼·Â ¼ö¼ö·á, {mana} ¸¶³ª ¼ö¼ö·á");
    }

    public void GetReward(Reward reward)
    {
        PlayerStat stat = reward.playerStat;
        health += stat.health;
        mana += stat.mana;
        stamina += stat.stamina;
        Debug.Log($"{health} Ã¼·Â È¹µæ, {mana} ¸¶³ª È¹µæ, {stamina} ½ºÅ×¹Ì³ª È¹µæ");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerResource : IReward
{
    public int cash;
    public int gold;
    public int gem;

    public void CancelCharge(Reward reward)
    {
        Debug.Log($"{cash} Ä³½Ã ¼ö¼ö·á, {gold} °ñµå ¼ö¼ö·á, {gem} Áª ¼ö¼ö·á");
    }

    public void GetReward(Reward reward)
    {
        PlayerResource playerResource = reward.playerResource;
        cash += playerResource.cash;
        gold += playerResource.gold;
        gem += playerResource.gem;

        Debug.Log($"{cash} Ä³½Ã È¹µæ, {gold} °ñµå È¹µæ, {gem} Áª È¹µæ");
    }
}

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
        Debug.Log($"{cash} ĳ�� ������, {gold} ��� ������, {gem} �� ������");
    }

    public void GetReward(Reward reward)
    {
        PlayerResource playerResource = reward.playerResource;
        cash += playerResource.cash;
        gold += playerResource.gold;
        gem += playerResource.gem;

        Debug.Log($"{cash} ĳ�� ȹ��, {gold} ��� ȹ��, {gem} �� ȹ��");
    }
}

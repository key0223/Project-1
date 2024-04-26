using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHistory
{
    public string characterSkin; //ĳ���� ����
    public string characterBorn; //�¾ ��
    public string characterStory; //ĳ���Ͱ� ǥ���� ����� �� �ϴ� ��
    public string characterName; //ĳ���� �̸�
    public CharacterEmblem characterEmblem; //ǥ���� ����
    public string pyogukName; // ǥ�� �̸�
    public string chongGhwan; //�Ѱ�
    public string pyogukStartLocation; //ǥ��������ġ

    public void SetPlayerHistory(PlayerHistory playerHistory)
    {
        characterSkin = playerHistory.characterSkin;
        characterBorn = playerHistory.characterBorn;
        characterStory = playerHistory.characterStory;
        characterName = playerHistory.characterName;
        characterEmblem = playerHistory.characterEmblem;
        pyogukName = playerHistory.pyogukName;
        chongGhwan = playerHistory.chongGhwan;
        pyogukStartLocation = playerHistory.pyogukStartLocation;
    }
}

[System.Serializable]
public struct CharacterEmblem
{
    public string emblem;
    public string emblemBackground;
    public string emblemColor1;
    public string emblemColor2;
}

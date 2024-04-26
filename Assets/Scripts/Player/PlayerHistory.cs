using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHistory
{
    public string characterSkin; //캐릭터 외형
    public string characterBorn; //태어난 곳
    public string characterStory; //캐릭터가 표국을 세우기 전 하던 일
    public string characterName; //캐릭터 이름
    public CharacterEmblem characterEmblem; //표국의 문양
    public string pyogukName; // 표국 이름
    public string chongGhwan; //총관
    public string pyogukStartLocation; //표국시작위치

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Chonghawn
{
    public string chonghawnName;
    public string chonghawnEffect;
    public int chonghawnValue;
}
[System.Serializable]
public class chonghawnList
{
    public List<Chonghawn> chonghawns;
}
public class ChooseCharacterChonghawnPanel : PlayerSetupPanel_ChildPanel
{
    chonghawnList chonghawnList;
    IList<Sprite> chonghawnSkins;

    [Header("Inspector Set")]
    public Transform skinRoot;
    public GameObject chonghawnPrefab;
    public Text chonghawnEffetText;
    public PlayerChonghawnItem chonghawnItem; // 왼쪽 고른 이미지 설정

    bool characterSelected;

    public override void Start()
    {
        nextButton.interactable = characterSelected;
        base.Start();
        SetChonghawn();
    }

    //UI
    public void SetChonghawnData(Sprite sprite, Chonghawn chonghawn)
    {
        characterSelected = true;
        nextButton.interactable = characterSelected;

        chonghawnEffetText.text = $"총관 효과: \n" +
            $"{chonghawn.chonghawnEffect} + {chonghawn.chonghawnValue.ToString()} %";

        chonghawnItem.SetUp(null, sprite, chonghawn);
    }

    async void SetChonghawn()
    {
        chonghawnSkins = await ResourceManager.LoadResources<Sprite>("PlayerSkin");

        TextAsset textAsset = await ResourceManager.LoadResource<TextAsset>("ChonghawnEffect");
        this.chonghawnList = JsonUtility.FromJson<chonghawnList>(textAsset.text);

        CreateChonghawn();
    }

    void CreateChonghawn()
    {
        for (int i = 0; i < chonghawnSkins.Count-1; i++)
        {
            GameObject go = Instantiate(chonghawnPrefab, skinRoot);
            PlayerChonghawnItem chonghawn = go.GetComponent<PlayerChonghawnItem>();

            chonghawn.SetUp(this, chonghawnSkins[i], chonghawnList.chonghawns[i]);
        }
    }

    public override void Next()
    {
        base.Next();
    }

    public override void Previous()
    {
        base.Previous();
    }

    public override void BtnText()
    {
        base.BtnText();
    }

    public override void SetPanelText()
    {
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Chonghawn");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Chonghawn");
    }

}

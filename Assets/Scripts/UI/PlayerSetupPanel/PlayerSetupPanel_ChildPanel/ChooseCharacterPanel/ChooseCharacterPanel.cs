using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterPanel : PlayerSetupPanel_ChildPanel
{
    public string characterSkin { get; set; }

    IList<Sprite> characterSkins;

    [Header("Inspector Set")]
    public Transform skinRoot;
    public GameObject skinPrefab;
    public PlayerSkinItem chooseImage; //고른 이미지 설정

    public bool characterSelected;


   
    public override void Start()
    {
        nextButton.interactable = characterSelected;
       

        base.Start();
        SetCharacterSkin();
    }

    public void SetChooseImage(Sprite sprite)
    {
        //다음 버튼 활성화
        characterSelected = true;
        nextButton.interactable = characterSelected;

        chooseImage.SetUp(null, sprite);
    }

    async void SetCharacterSkin()
    {
        characterSkins = await ResourceManager.LoadResources<Sprite>("PlayerSkin");

        CreateSkins();
    }

    void CreateSkins()
    {
        for (int i = 0; i < characterSkins.Count; i++)
        {
            GameObject go = Instantiate(skinPrefab, skinRoot);
            PlayerSkinItem skin = go.GetComponent<PlayerSkinItem>();
            skin.SetUp(this, characterSkins[i]);
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
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Appear");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Appear");
    }
}

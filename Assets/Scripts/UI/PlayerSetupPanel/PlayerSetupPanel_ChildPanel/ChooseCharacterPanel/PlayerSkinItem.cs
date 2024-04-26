using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkinItem : MonoBehaviour
{
    public ChooseCharacterPanel chooseCharacter;
    public Image image;
    public Button button;
    public void SetUp(params object[] objects)
    {
        this.chooseCharacter = (ChooseCharacterPanel)objects[0];
        image.sprite = (Sprite)objects[1];
        SetButton();
    }

    void SetButton()
    {
        if (button == null) return;

        button.onClick.AddListener(() =>
        {
            chooseCharacter.characterSelected = true;
            chooseCharacter.SetChooseImage(image.sprite);
            PlayerSetupPanel.ins.playerHistory.characterSkin = image.sprite.name;
        });
    }

    //public void SetPlayerSkin()
    //{
    //    PlayerSetupPanel.ins.playerHistory.characterSkin = image.sprite.ToString();
    //}
}

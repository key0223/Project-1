using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBornItem : MonoBehaviour
{
    public ChooseBornPanel chooseBorn;
    public Image image;
    public string bornPlaceName;
    public string placeDescription;

    public Button bornBtn;
    public void SetUp(params object[] objects)
    {
        this.chooseBorn = (ChooseBornPanel)objects[0];
        image.sprite = (Sprite)objects[1];
        bornPlaceName = (string)objects[2];
        placeDescription = (string)objects[3];

        SetButton();
    }

    void SetButton()
    {
        if (bornBtn == null) return;

        bornBtn.onClick.AddListener(() =>
        {
            chooseBorn.placeSelected = true;
            chooseBorn.SetBornPlaceData(image.sprite, bornPlaceName, placeDescription);
            PlayerSetupPanel.ins.playerHistory.characterBorn = bornPlaceName;
        });
    }
}

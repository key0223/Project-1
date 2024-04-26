using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChonghawnItem : MonoBehaviour
{
     ChooseCharacterChonghawnPanel chooseChonghawn;
     Chonghawn chonghawn;

    [Header("Inspector Set")]
    public Image image;
    public int chonghawnValue;
    public Button button;

   public void SetUp(params object[] objects)
    {
        this.chooseChonghawn = (ChooseCharacterChonghawnPanel)objects[0];
        image.sprite =(Sprite)objects[1];
        chonghawn = (Chonghawn)objects[2];
            
        SetButton();
    }

    void SetButton()
    {
        if (button == null) return;

        button.onClick.AddListener(() =>
        {
            chooseChonghawn.SetChonghawnData(image.sprite,chonghawn);
            PlayerSetupPanel.ins.playerHistory.chongGhwan = image.sprite.name;
        });
    }
}

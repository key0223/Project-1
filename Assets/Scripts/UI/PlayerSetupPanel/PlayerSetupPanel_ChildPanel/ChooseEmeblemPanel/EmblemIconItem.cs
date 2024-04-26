using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmblemIconItem : MonoBehaviour
{
    [Header("Inspector Set")]
    //임의 설정
    public SetPlayerEmblemPanel chooseEmblem;
    public Image emblemImage;
    public Button button;

    void Start()
    {
        SetButton();
    }
    public void SetButton()
    {
        if (button == null) return;
        button.onClick.AddListener(() =>
        {
            chooseEmblem.ChangeEmblemIcon(this);
        });
    }
}

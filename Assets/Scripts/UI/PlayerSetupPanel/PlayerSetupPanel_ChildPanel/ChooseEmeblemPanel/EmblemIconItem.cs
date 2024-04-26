using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmblemIconItem : MonoBehaviour
{
    [Header("Inspector Set")]
    //���� ����
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

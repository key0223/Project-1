using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmblemColorPartItem : MonoBehaviour
{
    [Header("Inspector Set")]
    public SetPlayerEmblemPanel playerEmblemPanel;
    public Button button;
    public EmblemPart emblemPart;
    void Start()
    {
        SetButton();
    }

    public void SetButton()
    {
        if (button == null) return;

        button.onClick.AddListener(() =>
        {
            playerEmblemPanel.ChagePart(this);

        });
    }
}

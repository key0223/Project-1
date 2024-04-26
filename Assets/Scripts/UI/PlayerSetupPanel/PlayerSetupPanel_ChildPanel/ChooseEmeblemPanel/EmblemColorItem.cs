using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmblemColorItem : MonoBehaviour
{
    public Color color;
    [Header("Inspector Set")]
    public SetPlayerEmblemPanel playerEmblemPanel;
    public Button colorBtn;

    void Start()
    {
        color = GetComponent<Image>().color;
        SetButton();
    }

    public void SetButton()
    {
        if (colorBtn == null) return;

        colorBtn.onClick.AddListener(() =>
        {
            playerEmblemPanel.ChagneColor(color);
        });
    }
}

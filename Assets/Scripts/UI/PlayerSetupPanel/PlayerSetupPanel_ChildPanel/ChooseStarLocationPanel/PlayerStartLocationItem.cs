using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStartLocationItem : MonoBehaviour
{
    public ChooseStartLocationPanel locationPanel;

    [Header("InspectorSet")]
    public Button startLocationBtn;
    public Text startLocationText;

    private void Start()
    {
        SetButton();
    }
    //public void SetUp(params object[] objects)
    //{
    //    this.locationPanel = (ChooseStartLocationPanel)objects[0];
    //    startLocationText.text = startLocationText.text;

    //}

    void SetButton()
    {
        if (startLocationBtn == null) return;

        startLocationBtn.onClick.AddListener(() =>
        {
            locationPanel.SetStartLocation();
            PlayerSetupPanel.ins.playerHistory.pyogukStartLocation = startLocationText.text;
        });
    }
}

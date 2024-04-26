using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseStartLocationPanel : PlayerSetupPanel_ChildPanel
{
    public PlayerStartLocationItem chooseLocation;

    [Header("Inspector Set")]
    public Transform locationRoot;

    bool locationSelected;

    public override void Start()
    {
        nextButton.interactable = locationSelected;
        base.Start();

        
    }

    public void SetStartLocation()
    {
        locationSelected = true;
        nextButton.interactable = locationSelected;

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
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Location");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Location");

    }
}

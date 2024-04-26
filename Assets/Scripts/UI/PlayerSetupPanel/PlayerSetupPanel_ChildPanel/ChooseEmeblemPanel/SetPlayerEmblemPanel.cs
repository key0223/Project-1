using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using UnityEngine;
using UnityEngine.UI;

public enum EmblemPart
{
    PART_BACKGROUND,
    PART_ICON,
}

public class SetPlayerEmblemPanel : PlayerSetupPanel_ChildPanel
{
    EmblemIconItem emblemIconItem;
    EmblemPart emblemPart;
    string playerName;

    [Header("Inspector Set")]
    public Image emblemBackgroundImage;
    public Image emblemIconImage;
    public Text playerNameText;

    [Header("UI Text")]
    [SerializeField] Text part1BtnText;
    [SerializeField] Text part2BtnText;


    bool emblemSelected;

    public override void Start()
    {
        nextButton.interactable = emblemSelected;

        playerName = PlayerSetupPanel.ins.playerHistory.characterName;
        playerNameText.text = playerName;
        base.Start();
    }

    public void ChangeEmblemIcon(EmblemIconItem icon)
    {
        emblemSelected = true;
        nextButton.interactable = emblemSelected;

        emblemIconItem = icon;
        emblemIconImage.sprite = icon.emblemImage.sprite;
        SetEmblem();
    }
    public void ChagePart(EmblemColorPartItem emblemColorPart)
    {
        this.emblemPart = emblemColorPart.emblemPart;
    }

    public void ChagneColor(Color color)
    {
        switch (emblemPart)
        {
            case EmblemPart.PART_BACKGROUND:
                emblemBackgroundImage.color = color;
                break;
            case EmblemPart.PART_ICON:
                emblemIconImage.color = color;
                break;
        }

        SetEmblem();
    }

    void SetEmblem()
    {
        PlayerSetupPanel.ins.playerHistory.characterEmblem.emblemBackground = emblemBackgroundImage.sprite.name;
        PlayerSetupPanel.ins.playerHistory.characterEmblem.emblem = emblemIconImage.sprite.name;

        PlayerSetupPanel.ins.playerHistory.characterEmblem.emblemColor1 = emblemBackgroundImage.color.ToString();
        PlayerSetupPanel.ins.playerHistory.characterEmblem.emblemColor2 = emblemIconImage.color.ToString();

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
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Emblem");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Emblem");
        part1BtnText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetEmblem_Part1");
        part2BtnText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetEmblem_Part2");

    }

}

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerPyogukNamePanel : PlayerSetupPanel_ChildPanel
{
    int minText = 2;
    int maxText = 6;

    string firebaseChild = "playerPyogukName";

    [Header("Inspector Set")]
    public InputField playerPyogukNameInput;
    public Text pyogukNameText;


    [Header("ErrorText")]
    public Text nameSettingsNoticeText;
    public Text nameInfoErrorText;
    public Text nameCheckBtnText;
    public Button nameCheckBtn;

    bool nameConfirmed;

    public override void Start()
    {
        nextButton.interactable = nameConfirmed;

        base.Start();
        Btn_Confirm();
    }
    public void Btn_Confirm()
    {
        if (nameCheckBtn == null) return;

        nameCheckBtn.onClick.AddListener(async () =>
        {
            await CheckPlayerPyogukNameAsync(playerPyogukNameInput.text);
        });
    }

    async Task CheckPlayerPyogukNameAsync(string pyogukName)
    {
        //¹®ÀÚ, ¼ýÀÚ, ÇÑ±ÛÀÌ ¾Æ´Ñ ¹®ÀÚµéÀ» ±æÀÌ 1-6±îÁö Ã£¾Æ¼­ ºó ¹®ÀÚ¿­·Î ´ëÃ¼
        string nameChecker = Regex.Replace(pyogukName, @"[^a-zA-Z0-9°¡-ÆR]{1,6}", "", RegexOptions.Singleline);

        nameConfirmed = false;

        bool data = await Manager.ins.firebaseFindData.CheckifNameExists(firebaseChild, playerPyogukNameInput.text);

        if (data)
        {
            nameConfirmed = false;
            nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_Duplicate");
            playerPyogukNameInput.text = string.Empty;
        }
        else
        {
            if (playerPyogukNameInput.text.Equals(nameChecker) && playerPyogukNameInput.text.Length >= minText && playerPyogukNameInput.text.Length <= maxText)
            {

                nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_Enable");
                nameConfirmed = true;

                pyogukNameText.text = playerPyogukNameInput.text;
                PlayerSetupPanel.ins.playerHistory.pyogukName = pyogukNameText.text;
            }
            else if (playerPyogukNameInput.text.Length < minText + 1 || playerPyogukNameInput.text.Length >= maxText - 1)
            {
                playerPyogukNameInput.text = string.Empty;

                nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_NameLimit");
                nameConfirmed = false;
            }
            else
            {
                playerPyogukNameInput.text = string.Empty;

                nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_SpecialSymbol");
                nameConfirmed = false;
            }
        }

        nextButton.interactable = nameConfirmed;
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
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Country");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Country");
        nameSettingsNoticeText.text = Manager.ins.stringTableManager.GetValue("NicknameSettingsNotice");
        nameCheckBtnText.text = Manager.ins.stringTableManager.GetValue("Conf_Name");
    }
}

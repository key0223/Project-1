using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase.Database;

public class SetPlayerNamePanel : PlayerSetupPanel_ChildPanel
{
    int minText = 2;
    int maxText = 8;

    string firebaseChild = "playerName";

    [Header("Inspector Set")]
    public InputField playerNameInput;
    //UI
    public Text playerNameText;

    [Header("ErrorText")]
    public Text nameSettingsNoticeText;
    public Text nameInfoErrorText;
    public Text nameCheckBtnText;
    public Button nameCheckBtn;

    public bool nameConfirmed;

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
           //Manager.ins.playerNameDatabase.Save(playerNameInput.text);
           await CheckPlayerNameAsync(playerNameInput.text);
       });
    }


    async Task CheckPlayerNameAsync(string playerName)
    {
        //¿µ¾î, ÇÑ±¹¾î, ¼ýÀÚ¸¦ Á¦¿ÜÇÑ ´Ù¸¥ ¹®ÀÚ Á¦°Å ¹× ±ÛÀÚÁ¦ÇÑ
        string nameChecker = Regex.Replace(playerName, @"[^a-zA-Z0-9°¡-ÆR]{1,8}", "", RegexOptions.Singleline);

        nameConfirmed = false;

        FirebaseDatabase.DefaultInstance.GetReference("players").KeepSynced(false);

        //µ¥ÀÌÅÍ°¡ ÀÖ´Â °æ¿ì
        bool data = await Manager.ins.firebaseFindData.CheckifNameExists(firebaseChild, playerNameInput.text);

        if (data)
        {
            nameConfirmed = false;
            nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_Duplicate");
            playerNameInput.text = string.Empty;
        }
        else if (playerNameInput.text.Equals(nameChecker) && playerNameInput.text.Length >= minText && playerNameInput.text.Length <= maxText)
        {
            nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_Enable");
            nameConfirmed = true;

            playerNameText.text = playerNameInput.text;
            PlayerSetupPanel.ins.playerHistory.characterName = playerNameText.text;
        }
        else if (playerNameInput.text.Length < minText + 1 || playerNameInput.text.Length >= maxText - 1)
        {
            playerNameInput.text = string.Empty;
            nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_NameLimit");
            nameConfirmed = false;
        }
        else
        {
            playerNameInput.text = string.Empty;
            nameInfoErrorText.text = Manager.ins.stringTableManager.GetValue("InfoError_SpecialSymbol");
            nameConfirmed = false;
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
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Name");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Name");
        nameSettingsNoticeText.text = Manager.ins.stringTableManager.GetValue("NicknameSettingsNotice");
        nameCheckBtnText.text = Manager.ins.stringTableManager.GetValue("Conf_Name");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PlayerSetupPanel_ChildPanel
{
    [SerializeField] Text endText;
    [SerializeField] City_MainUI city_MainUI;
    public override void Start()
    {
        Next();
        base.Previous();
        BtnText();
        SetPanelText();

        city_MainUI = FindObjectOfType<City_MainUI>();
    }

    public override void Next()
    {
        if (nextButton == null)
            return;

        nextButton.onClick.AddListener(() =>
        {
            if (nextPanel == null)
                return;

            PlayerSetupPanel.ins.SetInitialPlayerHistory();
            
            Destroy(nextPanel.gameObject);
            //gameObject.SetActive(false);
            //임시코드
            city_MainUI.OnGameStart();
        });
    }

    public override void Previous()
    {
        if (previousButton == null)
            return;

        previousButton.onClick.AddListener(() =>
        {
            if (previousPanel == null)
                return;

            previousPanel.SetActive(true);
            gameObject.SetActive(false);
        });

    }
    public override void BtnText()
    {
        nextBtnText.text = Manager.ins.stringTableManager.GetValue("Start");
        prevBtnText.text = Manager.ins.stringTableManager.GetValue("Prev");
    }
    public override void SetPanelText()
    {
        endText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_End");
    }
}

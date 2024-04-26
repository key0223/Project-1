using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupPanel_ChildPanel : MonoBehaviour
{
    [SerializeField] protected GameObject previousPanel;
    [SerializeField] protected GameObject nextPanel;

    [SerializeField] protected Button previousButton;
    [SerializeField] protected Button nextButton;

    [Header("PanelText")]
    [SerializeField] protected Text nextBtnText;
    [SerializeField] protected Text prevBtnText;
    [SerializeField] protected Text panelTitleText;
    [SerializeField] protected Text panelSubTitleText;
    public virtual void Start()
    {
        Next();
        Previous();
        BtnText();
        SetPanelText();
    }

    public virtual void Next()
    {
        if (nextButton == null)
            return;

        nextButton.onClick.AddListener(() =>
        {
            if (nextPanel == null)
                return;

            nextPanel.SetActive(true);
            gameObject.SetActive(false);
        });

        
    }

    public virtual void Previous()
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

    public virtual void BtnText()
    {
        nextBtnText.text = Manager.ins.stringTableManager.GetValue("Next");
        prevBtnText.text = Manager.ins.stringTableManager.GetValue("Prev");
    }

    public virtual void SetPanelText()
    {
        panelTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetTitle_Appear");
        panelSubTitleText.text = Manager.ins.stringTableManager.GetValue("PlayerInitSetSubTitle_Appear");
    }
        
    //public virtual void Confirm()
    //{
    //    nextButton
    //}
}

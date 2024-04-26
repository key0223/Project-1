using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressOptionPanel : MonoBehaviour
{
    #region StringTable
    [Header("StringTables")]
    [SerializeField] Text titleQuestChosseProgressText;
    [SerializeField] Text subtitleText;
    [SerializeField] Text confirmBtnText;
  
    [SerializeField] string stateRich;
    [SerializeField] string statePoor;
    [SerializeField] string stateEpidemic;
    [SerializeField] string stateInsurrection;

    #endregion

    //코드 셋팅
    [Space(20)]
    [SerializeField] ProgressOptionType progressOptionType;
    [SerializeField] ProgressEvent progressEvent;


    [Header("Inspector Set")]
    [SerializeField] GameObject questDirectingPanel;
    [SerializeField] Text locationNameText;
    [SerializeField] Text populationText;
    [SerializeField] Text securityText;
    [SerializeField] Text locationState;

    [SerializeField] Button confirmBtn;

    ToggleGroup toggleGroup;
    public QuestTracker Owner { get; private set; }

    private void Awake()
    {
        titleQuestChosseProgressText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestChooseProgress");
        subtitleText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestChooseProgressSubtitle");
        confirmBtnText.text = Manager.ins.stringTableManager.GetValue("Confirm");

        toggleGroup = GetComponent<ToggleGroup>();
        confirmBtn.onClick.AddListener(() => OnAcceptBtn());

        progressEvent = Manager.ins.questEventManager.ProgressEvent;
    }


    public void Show(QuestTracker questTracker,Dest dest)
    {
        questDirectingPanel.SetActive(false);
        toggleGroup.SetAllTogglesOff();
        Owner = questTracker;
        locationNameText.text = $"{dest}";
        populationText.text = $"{progressEvent.PopulationValue}";
        securityText.text = $"{progressEvent.SecurityValue}";
        locationState.text = $"{progressEvent.LocationStateValue}";

        confirmBtn.interactable = false;
    }
    public void Hide()
    {
        Manager.ins.questEventManager.EndEvent();
        Owner = null;
        gameObject.SetActive(false);
    }
    public void OnOptionSelected(ProgressOptionType progressType)
    {
        if (Owner == null)
            return;

        progressOptionType = progressType;
        confirmBtn.interactable = true;
    }

    void OnAcceptBtn()
    {
        if (confirmBtn == null) return;

        //Owner.OnProgressOption(progressOptionType);
        progressEvent.OnOptionSelected(progressOptionType);
        Hide();
        questDirectingPanel.SetActive(true);

    }


}

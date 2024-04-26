using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressOptionIneui : QuestOption
{
    #region StringTable
    string moveSpeed;
    string encounterProbability;
    string battleConfidence;

    string speed;
    string probability;
    string confidence;
    #endregion

    QuestProgressOptionPanel questProgressOptionPanel;

    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] string effectSpeed;
    [SerializeField] string effectThief;
    [SerializeField] string effectConfidence;

    protected ProgressOptionType progressOptionType;


    protected override void Start()
    {
        moveSpeed = Manager.ins.stringTableManager.GetValue("MoveSpeed");
        encounterProbability = Manager.ins.stringTableManager.GetValue("EncounterProbability");
        battleConfidence = Manager.ins.stringTableManager.GetValue("BattleConfidence");
        speed = Manager.ins.stringTableManager.GetValue("Normal");
        probability = Manager.ins.stringTableManager.GetValue("Normal");
        confidence = Manager.ins.stringTableManager.GetValue("Normal");

        questProgressOptionPanel = gameObject.GetComponentInParent<QuestProgressOptionPanel>();
        optionToggle = GetComponent<Toggle>();
        optionToggle.group = questProgressOptionPanel.GetComponent<ToggleGroup>();

        optionToggle.onValueChanged.AddListener(isOn => OnSelected(isOn));
        Setup();
    }
    protected override void Setup()
    {
        progressOptionType = ProgressOptionType.Ineui;

        title = Manager.ins.stringTableManager.GetValue("Quest_ProgressOption_Ineui");
        description = Manager.ins.stringTableManager.GetValue("Quest_IneuiDesc");
        effectSpeed = $"{moveSpeed} {speed}";
        effectThief = $"{encounterProbability} {probability}";
        effectConfidence = $"{battleConfidence} {confidence}";

        titleText.text = title;
        descriptionText.text = description;
        effectText.text = $"{effectSpeed}\n" + $"{effectThief}\n" + $"{effectConfidence}";
    }
    protected override void OnSelected(bool isOn)
    {
        if (isOn)
        {
            selectedImage.SetActive(true);
            questProgressOptionPanel.OnOptionSelected(progressOptionType);
        }
        else
            selectedImage.SetActive(false);
    }
}

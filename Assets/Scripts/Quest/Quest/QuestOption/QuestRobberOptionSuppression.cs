using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRobberOptionSuppression : QuestOption
{
    #region StringTable
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] string effect1;
    [SerializeField] string effect2;
    #endregion

    QuestRobberEventPanel questRobberEventPanel;
   
    [SerializeField] RobberOptionType robberOptionType;

    protected override void Start()
    {
        questRobberEventPanel = gameObject.GetComponentInParent<QuestRobberEventPanel>();

        optionToggle = GetComponent<Toggle>();
        optionToggle.group = questRobberEventPanel.GetComponent<ToggleGroup>();

        optionToggle.onValueChanged.AddListener(isOn => OnSelected(isOn));
        Setup();
    }
    protected override void Setup()
    {
        robberOptionType = RobberOptionType.Suppression;

        title = Manager.ins.stringTableManager.GetValue("Suppression");
        description = Manager.ins.stringTableManager.GetValue("RobberEvent_SuppDesc");
        effect1 = Manager.ins.stringTableManager.GetValue("SuppEffect1");
        effect2 = Manager.ins.stringTableManager.GetValue("SuppEffect2");

        titleText.text = title;
        descriptionText.text = description;
        effectText.text = $"{effect1}\n" + $"{effect2}";

    }
    protected override void OnSelected(bool isOn)
    {
        if (isOn)
        {
            selectedImage.SetActive(true);
            questRobberEventPanel.OnOptionSelected(optionToggle, robberOptionType);
        }
        else
            selectedImage.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRobberOptionPayment : QuestOption
{
    QuestRobberEventPanel questRobberEventPanel;

    #region StringTable
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] string effect1;
    #endregion


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
        robberOptionType = RobberOptionType.Payment;

        title = Manager.ins.stringTableManager.GetValue("Payment");
        description = Manager.ins.stringTableManager.GetValue("RobberEvent_PayDesc");
        effect1 = Manager.ins.stringTableManager.GetValue("PayEffect1");

        titleText.text = title;
        descriptionText.text = description;
        effectText.text = $"{effect1}";

    }
    protected override void OnSelected(bool isOn)
    {
        if (isOn)
        {
            selectedImage.SetActive(true);
            questRobberEventPanel.OnOptionSelected(optionToggle,robberOptionType);
        }
        else
            selectedImage.SetActive(false);
    }

  
}

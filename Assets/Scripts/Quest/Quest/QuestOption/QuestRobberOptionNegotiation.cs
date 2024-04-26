using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRobberOptionNegotiation : QuestOption
{
    #region StringTable
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] string effect1;
    [SerializeField] string effect2;
    [SerializeField] string effect3;
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
        robberOptionType = RobberOptionType.Negotiation;

        title = Manager.ins.stringTableManager.GetValue("Negotiation");
        description = Manager.ins.stringTableManager.GetValue("RobberEvent_NegoDesc");
        effect1 = Manager.ins.stringTableManager.GetValue("NegoEffect1");
        effect2 = Manager.ins.stringTableManager.GetValue("NegoEffect2");
        effect3 = Manager.ins.stringTableManager.GetValue("NegoEffect3");

        titleText.text = title;
        descriptionText.text = description;
        effectText.text = $"{effect1}\n" + $"{effect2}\n" + $"{effect3}";
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

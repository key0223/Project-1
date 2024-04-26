using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AvailableQuestList : MonoBehaviour
{
    [SerializeField] private QuestDetailPanel questDetail;

    ToggleGroup toggleGroup;
    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();

        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(isOn => OnToggleValueChanged(toggle, isOn));
        }
    }

    public void OnToggleValueChanged(Toggle toggle, bool isOn)
    {
        if (isOn)
            questDetail.gameObject.SetActive(true);
        else
            questDetail.gameObject.SetActive(false);
    }

    public void AddToAvalilableListView(Quest quest, UnityAction<bool> onClicked)
    {
        Toggle toggle = quest.gameObject.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(onClicked);
    }

}

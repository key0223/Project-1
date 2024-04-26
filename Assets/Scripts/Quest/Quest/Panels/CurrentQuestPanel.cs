using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentQuestPanel : MonoBehaviour
{
    [Header("StringTable")]
    [SerializeField] Text titleCurrentQuestText;

    [Header("Inspector Set")]
    [SerializeField] QuestProgressOptionPanel progressOptionPanel;
    [SerializeField] QuestRobberEventPanel robberEventPanel;
    [SerializeField] QuestDirectingPanel questDirectingPanel;
    [SerializeField] QuestTracker questTrackerPrefab;
    [SerializeField] Transform trackerParent;

    private void Start()
    {
        titleCurrentQuestText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestCurrentQuest");

        QuestSystem.Instance.onQuestRegistered += CreateQuestTracker;

        foreach(var quest in QuestSystem.Instance.ActiveQuests)
            CreateQuestTracker(quest);

    }
    private void OnDestroy()
    {
        var questSystem = QuestSystem.Instance;
        if(questSystem)
        {
            questSystem.onQuestRegistered -= CreateQuestTracker;
        }
    }
    void CreateQuestTracker(Quest quest)
    {
        Instantiate(questTrackerPrefab, trackerParent).Setup(quest, progressOptionPanel,robberEventPanel,questDirectingPanel);
    }


}

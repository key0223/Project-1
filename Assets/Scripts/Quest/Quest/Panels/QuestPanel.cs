using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    #region StringTable
    [Header("StringTables")]
    [SerializeField] Text titleQuetListText;
    [SerializeField] Text subtitleQuestTypeText;
    [SerializeField] Text subtitleQuestOwnerText;
    [SerializeField] Text subtitleQuestContentText;
    [SerializeField] Text subtitleQuestDestText;
    [SerializeField] Text subtitleQuestDeadlineText;
    [SerializeField] Text subtitleQuestRewardText;
    #endregion

    [SerializeField] QuestDetailPanel questDetail;
    [SerializeField] private AvailableQuestList availableQuestList;


    private void Awake()
    {
        titleQuetListText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestList");
        subtitleQuestTypeText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestType");
        subtitleQuestOwnerText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestOwner");
        subtitleQuestContentText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestContent");
        subtitleQuestDestText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestDest");
        subtitleQuestDeadlineText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestDeadline");
        subtitleQuestRewardText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestReward");
    }

    private void Start()
    {
        var questSystem = QuestSystem.Instance;

        foreach (var quest in questSystem.AvailableQuests)
            AddQuestToAvalilableList(quest);

        questSystem.onQuestAvailable += AddQuestToAvalilableList;

    }

    private void OnEnable()
    {
        questDetail.gameObject.SetActive(false);
    }
    private void AddQuestToAvalilableList(Quest quest)
        => availableQuestList.AddToAvalilableListView(quest, isOn => ShowDetail(isOn, quest));

    private void ShowDetail(bool isOn, Quest quest)
    {
        if (isOn)
        {
            questDetail.Show(quest);
            questDetail.gameObject.SetActive(true);
        }
    }
    
}

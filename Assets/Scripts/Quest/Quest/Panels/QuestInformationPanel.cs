using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInformationPanel : MonoBehaviour
{
    #region StringTable
    [Header("StringTables")]
    [SerializeField] Text titleQuestInformationText;
    [SerializeField] Text subtitleTypeText;
    [SerializeField] Text subtitleOwnerText;
    [SerializeField] Text subtitleDeadlineText;
    [SerializeField] Text subtitlePathText;
    [SerializeField] Text subtitleEscortText;
    [SerializeField] Text subtitleEscortReaderText;
    [Space(10)]
    [SerializeField] Text subtitleEquipText;
    [SerializeField] Text horseText;
    [SerializeField] Text cartText;
    [SerializeField] Text shipText;
    [Space(10)]
    [SerializeField] Text subtitleLevelText;
    [SerializeField] Text subtitleDestText;
    [SerializeField] Text subtitleDepositText;
    [SerializeField] Text subtitleRewardText;
    [SerializeField] Text subtitleLaborerText;
    [SerializeField] Text subtitleBudgetText;
    [Space(10)]
    [SerializeField] Text cancelBtnText;
    [SerializeField] Text startBtnText;
    #endregion

    [Header("Panels")]
    [SerializeField] QuestPanel questPanel;
    [SerializeField] QuestDetailPanel questDetailPanel;
    [SerializeField] QuestChoosePyosaPanel questChoosePyosaPanel;
    #region Panel - UI Set
    [Header("Descriptons")]
    [SerializeField] Text typeDescText;
    [SerializeField] Text ownerDescText;
    [SerializeField] Text deadlineDescText;
    [SerializeField] GameObject[] path;
    [SerializeField] Text escortDescText;
    [SerializeField] Text pyoduNameText;
    [SerializeField] Text horseCountText;
    [SerializeField] Text cartCountText;
    [SerializeField] Text shipCountText;
    [Space(10f)]
    [SerializeField] Text levelDescText;
    [SerializeField] Text destText;
    [SerializeField] Text depositDescText;
    [SerializeField] Text rewardDescText;
    [SerializeField] Text laborerDescText;
    [SerializeField] Text budgetDescText;

    [Header("Buttons")]
    [SerializeField] Button cancelBtn;
    [SerializeField] Button startBtn;
    #endregion

    public Quest Target { get; private set; }

    int budget;

    private void Awake()
    {
        titleQuestInformationText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestDetail");
        //subtitleTypeText.text = Manager.ins.stringTableManager.GetValue("");
        subtitleOwnerText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestOwner");
        subtitleDeadlineText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestDeadline");
        subtitlePathText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestPath");
        subtitleEscortText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestEscort");
        subtitleEquipText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestEquipment");
        horseText.text = Manager.ins.stringTableManager.GetValue("Horse");
        cartText.text = Manager.ins.stringTableManager.GetValue("Cart");
        shipText.text = Manager.ins.stringTableManager.GetValue("Ship");
        subtitleLevelText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestLevel");
        subtitleDestText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestDest");
        subtitleDepositText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestDeposit");
        subtitleRewardText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestSuccessReward");
        subtitleLaborerText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestLaborer");
        subtitleBudgetText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestBudget");

        cancelBtnText.text = Manager.ins.stringTableManager.GetValue("Cancel");
        startBtnText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestStart");
    }
    public void Setup(Quest quest)
    {
        Target = quest;

        typeDescText.text = quest.QuestDesc;
        ownerDescText.text = quest.OwnerName;
        deadlineDescText.text = $"{quest.Deadline}";
        escortDescText.text = $"{quest.questUnit.Escort}";
        pyoduNameText.text = $"{quest.Pyodu.PyosaName}";
        SetPath(quest);

        escortDescText.text = $"{(quest.AttendedEscortList.Count - 1)}";
        horseCountText.text = $"99/{quest.questUnit.Horse}";
        cartCountText.text = $"99/{quest.questUnit.Cart}";
        shipCountText.text = $"99/{quest.questUnit.Ship}";
        levelDescText.text = $"{quest.QuestLevel}";
        destText.text = $"{quest.Dest}";
        depositDescText.text = $"{quest.questUnit.Deposit}";
        rewardDescText.text = $"{quest.questUnit.Remainder}";
        laborerDescText.text = $"{quest.questUnit.Laborer}";

        budget = SetBudget(quest);
        budgetDescText.text = $"{budget}";
        startBtn.onClick.AddListener(() => OnStartBtn());
        cancelBtn.onClick.AddListener(() => OnCancelBtn());
    }
    void SetPath(Quest quest)
    {
        for (int i = 0; i < quest.routes.Length; i++)
        {
            if (i < quest.routes.Length - 1)
            {
                path[i].gameObject.SetActive(true);

                Text text = path[i].GetComponentInChildren<Text>();
                text.text = $"{quest.routes[i].Dest}";
            }

        }
    }
    void PathDeactive()
    {
        for (int i = 0; i < path.Length; i++)
        {
            path[i].gameObject.SetActive(false);
        }
    }
    int SetBudget(Quest quest)
    {
        int budgetSum;
        int equipBudget;
        int horseBudget;
        int cartBudget;
        int shipBudget;
        int laborerBudget;

        horseBudget = (int)(quest.questUnit.Horse * 5 * quest.Deadline * 1.5);
        cartBudget = quest.questUnit.Cart * 100;
        shipBudget = quest.questUnit.Ship * 5000;

        equipBudget = horseBudget + cartBudget + shipBudget;
        laborerBudget = (int)(quest.questUnit.Laborer * quest.Deadline * 1.5);

        budgetSum = equipBudget + laborerBudget;

        return budgetSum;
    }
    public void OnCancelBtn()
    {
        Target = null;
        questChoosePyosaPanel.gameObject.SetActive(true);

        PathDeactive();
        gameObject.SetActive(false);
    }
    void OnStartBtn()
    {
        if (QuestSystem.Instance.ContainsInActiveQuests(Target))
            return;
        else
        {
            Target.OnRegister();

           foreach(var escort in questChoosePyosaPanel.TempEscortList)
            {
                questChoosePyosaPanel.RegisteredEscortList.Add(escort);
            }

            questChoosePyosaPanel.TempEscortList.Clear();
        }

        PathDeactive();
        questPanel.gameObject.SetActive(false);
        questDetailPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);

    }
}

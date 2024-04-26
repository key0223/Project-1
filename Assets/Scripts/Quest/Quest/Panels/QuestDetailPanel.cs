using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailPanel : MonoBehaviour
{
    #region Detail Panel - UI Text
    //인스펙터 세팅
    [Header("Title Text Set")]
    [SerializeField] Text titleQuestDetatilText;
    [SerializeField] Text subtitleTypeText;
    [SerializeField] Text subtitleOwnerText;
    [SerializeField] Text subtitleDeadlineText;
    [SerializeField] Text subtitlePathText;
    [SerializeField] Text subtitleEscortText;
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
    [Space(20)]
    [Header("Descriptons Text Set")]
    [SerializeField] Text typeDescText;
    [SerializeField] Text ownerDescText;
    [SerializeField] Text deadlineDescText;
    [SerializeField] Text escortDescText;
    [SerializeField] Text horseCountText;
    [SerializeField] Text cartCountText;
    [SerializeField] Text shipCountText;
    [SerializeField] Text levelDescText;
    [SerializeField] Text destText;
    [SerializeField] Text depositDescText;
    [SerializeField] Text rewardDescText;
    [SerializeField] Text laborerDescText;
    [Space(10)]
    [SerializeField] Text cancelBtnText;
    [SerializeField] Text acceptBtnText;
    #endregion

    //인스펙터 세팅
    [Header("Buttons")]
    [SerializeField] Button cancelBtn;
    [SerializeField] Button acceptBtn;
    [SerializeField] QuestChoosePyosaPanel questChoosePyosaPanel;

    //코드세팅
    [Space(20)]
    [SerializeField] GameObject[] path;

    [Header("Temporary Data")]
    [SerializeField] int fame = 80; 
    
    public Quest Target { get; private set; }

    private void Awake()
    {
        titleQuestDetatilText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestDetail");
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
        cancelBtnText.text = Manager.ins.stringTableManager.GetValue("Cancel");
        acceptBtnText.text = Manager.ins.stringTableManager.GetValue("Quest_QuestAccept");


        gameObject.SetActive(false);

    }

    public void Show(Quest quest)
    {
        Target = quest;

        typeDescText.text = quest.QuestDesc;
        ownerDescText.text = quest.OwnerName;
        deadlineDescText.text = $"{quest.Deadline}";
        escortDescText.text = $"{quest.questUnit.Escort}";
       
        SetPath(quest);

        escortDescText.text = $"{quest.questUnit.Escort}";
        horseCountText.text = $"99/{quest.questUnit.Horse}";
        cartCountText.text = $"99/{quest.questUnit.Cart}";
        shipCountText.text = $"99/{quest.questUnit.Ship}";
        levelDescText.text = $"{quest.QuestLevel}";
        destText.text = $"{quest.Dest}";
        depositDescText.text = $"{quest.questUnit.Deposit}";
        rewardDescText.text = $"{quest.questUnit.Remainder}";
        laborerDescText.text = $"{quest.questUnit.Laborer}";

        acceptBtn.onClick.AddListener(() => OnQuestAccept());
        cancelBtn.onClick.AddListener(() => OnQuestCancel());
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
    public void Hide()
    {
        Target = null;
    }
    public void OnQuestAccept()
    {
        questChoosePyosaPanel.NeedEscort = Target.questUnit.Escort;
        questChoosePyosaPanel.SetQuest(Target);
        questChoosePyosaPanel.gameObject.SetActive(true);
        PathDeactive();
        gameObject.SetActive(false);
    }
    public void OnQuestCancel()
    {
        Target = null;
        PathDeactive();
        questChoosePyosaPanel.TempEscortList.Clear();
        gameObject.SetActive(false);
    }
}

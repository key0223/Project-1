using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QuestDirectingType
{
    OnQuestProgress,
    OnQuestRobberEvent,

}
public class QuestDirectingPanel : MonoBehaviour
{
    #region StringTable
    string directing_Progress;
    string directing_RobberEvent;
    string directing_RobberAttack;

    string negotiation;
    string suppression;
    string payment;
    string robberEvent_NegotiationSuccess;
    string robberEvent_NegotiationGreatFail;
    string robberEvent_NegotiationFail;
    string negotiation_GreatSuccess;
    string negotiation_Success;
    string negotiation_GreatFail;
    string negotiation_Fail;

    string robberEvent_Suppression;
    string robberEvent_Payment;

    string robberType_Mountain;
    string robberType_Sea;
    #endregion


    [SerializeField] Image directingImage;
    [SerializeField] Text directingText;

    public QuestRobberEventPanel questRobberEventPanel { get; private set; }

    [Header("임시 데이터")]
    [SerializeField] SuppressionResultPanel suppressionResultPanel;


    public QuestTracker Owner { get; private set; }
    NegotiationResult negoResult;

    private void Awake()
    {
        directingImage = GetComponentInChildren<Image>();
        directingText = GetComponentInChildren<Text>();
        directing_Progress = Manager.ins.stringTableManager.GetValue("Quest_QuestDirecting_Progress");
        directing_RobberEvent = Manager.ins.stringTableManager.GetValue("Quest_QuestDirecting_RobberEvent");
        directing_RobberAttack = Manager.ins.stringTableManager.GetValue("Quest_QuestDirecting_RobberAttack");

        negotiation = Manager.ins.stringTableManager.GetValue("Negotiation");
        suppression = Manager.ins.stringTableManager.GetValue("Suppression");
        payment = Manager.ins.stringTableManager.GetValue("Payment");

        robberEvent_NegotiationSuccess = Manager.ins.stringTableManager.GetValue("Quest_RobberEvent_NegotiationSuccess");
        robberEvent_NegotiationGreatFail = Manager.ins.stringTableManager.GetValue("Quest_RobberEvent_NegotiationGreatFail");
        robberEvent_NegotiationFail = Manager.ins.stringTableManager.GetValue("Quest_RobberEvent_NegotiationFail");

        robberEvent_Suppression = Manager.ins.stringTableManager.GetValue("Quest_RobberEvent_Suppression");
        robberEvent_Payment = Manager.ins.stringTableManager.GetValue("Quest_RobberEvent_Payment");

        robberType_Mountain = Manager.ins.stringTableManager.GetValue("RobberType_Mountain");
        robberType_Sea = Manager.ins.stringTableManager.GetValue("RobberType_Sea");

        negotiation_GreatSuccess = Manager.ins.stringTableManager.GetValue("GreatSuccess");
        negotiation_Success = Manager.ins.stringTableManager.GetValue("Success");
        negotiation_GreatFail = Manager.ins.stringTableManager.GetValue("GreatFail");
        negotiation_Fail = Manager.ins.stringTableManager.GetValue("Fail");

    }

    public void OnDirecting(ProgressOptionType option)
    {
        directingText.text = directing_Progress;
    }

    //RobberOptionType.Negotiation 일 때
    public void OnDirecting(QuestTracker questTracker, NegotiationResult negotiationResult)
    {

        Owner = questTracker;
        questRobberEventPanel = Owner.RobberEventPanel;
        negoResult = negotiationResult;
        float toll = Owner.Toll;

        if (negotiationResult == NegotiationResult.GreatSuccess)
        {
            directingText.text = string.Format(robberEvent_NegotiationSuccess, negotiation_GreatSuccess, toll, questRobberEventPanel.Robber.Type);
        }
        else if (negotiationResult == NegotiationResult.Success)
        {
            directingText.text = string.Format(robberEvent_NegotiationSuccess, negotiation_Success, toll, questRobberEventPanel.Robber.Type);
        }
        else if (negotiationResult == NegotiationResult.GreatFail)
        {
            directingText.text = string.Format(robberEvent_NegotiationGreatFail, negotiation_GreatFail);
        }
        else
        {
            directingText.text = string.Format(robberEvent_NegotiationFail, negotiation_Fail);
        }
    }

    public void OnDirecting(QuestTracker questTracker, RobberOptionType robberOptionType)
    {

        Owner = questTracker;

        questRobberEventPanel = questTracker.RobberEventPanel;

        if (robberOptionType == RobberOptionType.Suppression)
            directingText.text = string.Format(robberEvent_Suppression, questRobberEventPanel.Robber.RobberName, suppression);

        else if (robberOptionType == RobberOptionType.Payment)
            directingText.text = string.Format(robberEvent_Payment, questRobberEventPanel.Robber.RobberName, questTracker.Toll, questRobberEventPanel.Robber.Type);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (questRobberEventPanel == null)
                return;

            if (questRobberEventPanel.CurrentRobberOption == RobberOptionType.Negotiation)
            {
                if ((negoResult == NegotiationResult.GreatSuccess) || (negoResult == NegotiationResult.Success))
                {
                    Manager.ins.questEventManager.EndEvent();
                    OnDirecting(Manager.ins.questEventManager.ProgressEvent.CurrentProgressType);
                    Owner = null;
                }
                else if (negoResult == NegotiationResult.GreatFail)
                {
                    suppressionResultPanel.gameObject.SetActive(true);
                    suppressionResultPanel.Setup(Owner);

                    gameObject.SetActive(false);
                }
                else if (negoResult == NegotiationResult.Fail)
                {
                    questRobberEventPanel.gameObject.SetActive(true);
                    questRobberEventPanel.AcceptBtn.interactable = false;
                    gameObject.SetActive(false);
                }

            }
            else if (questRobberEventPanel.CurrentRobberOption == RobberOptionType.Suppression)
            {
                suppressionResultPanel.gameObject.SetActive(true);
                suppressionResultPanel.Setup(Owner);

                gameObject.SetActive(false);
            }
            else
            {
                CountryDate.Instance.TimeFlowingSet(true);
                //Owner.QuestReporter.StartReport();
                Owner = null;
                gameObject.SetActive(false);
            }
        }
    }
}

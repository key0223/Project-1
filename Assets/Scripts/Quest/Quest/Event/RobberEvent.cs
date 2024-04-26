using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Message;
public enum RobberOptionType
{
    Negotiation,
    Suppression,
    Payment,
    None,
}

public class RobberEvent : MonoBehaviour
{
    //코드 셋팅
    Quest targetQuest;
    QuestDirectingPanel directingPanel;
    ProgressEvent progressEvent;

    [Header("Robber Event")]
    [SerializeField] float encountProbability;
    [SerializeField] float toll;

    #region Properties
    public Quest TargetQuest => targetQuest;
    public float EncountProbability => encountProbability;
    #endregion

    private void Awake()
    {
        progressEvent = GetComponent<ProgressEvent>();
        directingPanel = GetComponent<QuestEventManager>().DirectingPanel;
    }
    public void Setup(Quest quest)
    {
        targetQuest = quest;
    }
    public void EncounterProbability(float security)
    {
        float multiplier = 0f;
        if (progressEvent.CurrentProgressType == ProgressOptionType.None)
            return;

        if (progressEvent.CurrentProgressType == ProgressOptionType.Wimoo)
            multiplier = 15f;
        else if (progressEvent.CurrentProgressType == ProgressOptionType.Ineui)
            multiplier = 10f;
        else
            multiplier = 5f;

        float newProbability = ((200 / security) - 2) * multiplier;
        float result = Mathf.Round(newProbability * 10) * 0.1f;

        encountProbability = result;
    }

    public void OnRobberOption(QuestTracker questTracker, RobberOptionType robberOption)
    {
        this.targetQuest = questTracker.TargetQuest;
        if (robberOption == RobberOptionType.Negotiation)
        {
            int CalculateValue()
            {
                int pyogukFame = Manager.ins.city_MainUI.fame;
                int questFame = TargetQuest.questUnit.Fame;
                int pyoduCharmValue = TargetQuest.Pyodu.Charm;
                int pyoduIntelligence = TargetQuest.Pyodu.Intelligence;
                int randValue = Random.Range(1, 26);

                int result = (pyogukFame / questFame) * (pyoduCharmValue / 100) * (pyoduIntelligence / 200) * 50 + randValue;
                return result;
            }

            int negotiationResultValue = CalculateValue();
            int tollReductionPercentage = 0;

            NegotiationResult resultType = NegotiationResult.GreatSuccess;

            #region SetNegotiationResult
            if (negotiationResultValue >= 40)
            {
                resultType = NegotiationResult.GreatSuccess;
                tollReductionPercentage = 50;
            }
            else if (negotiationResultValue >= 20 && negotiationResultValue < 40)
            {
                resultType = NegotiationResult.Success;
                tollReductionPercentage = 25;
            }
            else if (negotiationResultValue >= 10 && negotiationResultValue < 20)
            {
                resultType = NegotiationResult.Fail;
            }
            else if (negotiationResultValue < 10)
            {
                resultType = NegotiationResult.GreatFail;
            }
            #endregion

            toll = toll - Mathf.RoundToInt(toll * (tollReductionPercentage / 100f));
            directingPanel.OnDirecting(questTracker, resultType);
        }
        else
            directingPanel.OnDirecting(questTracker, robberOption);

        directingPanel.gameObject.SetActive(true);
    }
}




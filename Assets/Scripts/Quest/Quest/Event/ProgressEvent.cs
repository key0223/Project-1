using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProgressOptionType
{
    Wimoo,
    Ineui,
    Tupyo,
    None,
}
public class ProgressEvent : MonoBehaviour
{
    //코드 셋팅
    RobberEvent robberEvent;
    ProgressEvent progressEvent;

    [SerializeField] ProgressOptionType currentProgressType;

    [Header("임시 데이터"), Tooltip("통과하는 지역 정보")]
    int populationValue = 999999;
    int securityValue = 45;
    Special locationStateValue = Special.Rich;

    #region Properties
    public ProgressOptionType CurrentProgressType => currentProgressType;
    public int PopulationValue => populationValue;
    public int SecurityValue => securityValue;
    public Special LocationStateValue => locationStateValue;

    #endregion

    private void Start()
    {
        robberEvent = Manager.ins.questEventManager.RobberEvent;
        progressEvent = Manager.ins.questEventManager.ProgressEvent;

        currentProgressType = ProgressOptionType.None;
    }
    public void OnOptionSelected(ProgressOptionType type)
    {
       int date =  Manager.ins.questEventManager.QuestTracker.TargetQuest.CurrentRoute.Date;

        currentProgressType = type;

        if (currentProgressType == ProgressOptionType.None)
            return;

        float progressMultiplier = 1.0f;

        if (currentProgressType == ProgressOptionType.Wimoo)
            progressMultiplier = 0.8f;
        else if (currentProgressType == ProgressOptionType.Ineui)
            progressMultiplier = 0.9f;

        int decreaseValue = date - (Mathf.RoundToInt(date * progressMultiplier));

        Manager.ins.questEventManager.QuestTracker.UpdateQuestDate(decreaseValue);
        //Manager.ins.questEventManager.QuestTracker.UpdateQuestDate(progressMultiplier);

        //currentMaxValue = Mathf.RoundToInt(maxValue * progressMultiplier);
        robberEvent.EncounterProbability(securityValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class QuestTracker : MonoBehaviour
{
    [SerializeField] QuestProgressOptionPanel progressOptionPanel;

    //코드셋팅
    [SerializeField] Quest targetQuest;
    [SerializeField] QuestRobberEventPanel robberEventPanel;
    [SerializeField] QuestDirectingPanel questDirectingPanel;

    
    QuestDescriptor questDescriptor;

    [Header("Inspector Set")]
    [SerializeField] QuestDescriptor questDescriptorPrefab;

    Dictionary<Route, QuestDescriptor> questDesciptorByRoute = new Dictionary<Route, QuestDescriptor>();


    [Header("ProgressBar")]
    [SerializeField] Image progressBarImage;
    [SerializeField] float minValue = 0f;
    [SerializeField] float maxValue;
    [SerializeField] float currentMaxValue;
    [SerializeField] float currentValue = 0;

    [Header("Complete Quest - 임시")]
    [SerializeField] Button trackerBtn;
    [SerializeField] QuestCompletePanel completePanel;

    [SerializeField] float toll = 200;

    [SerializeField] Queue<Pyosa> getawayEscort = new Queue<Pyosa>();
    [SerializeField] Queue<Pyosa> seriousInjuryEscort = new Queue<Pyosa>();
    [SerializeField] Queue<Pyosa> slightInjuryEscort = new Queue<Pyosa>();
    [SerializeField] Queue<Pyosa> deathEscort = new Queue<Pyosa>();

    #region Proprties
    public Quest TargetQuest { get { return targetQuest; } }
    public QuestProgressOptionPanel ProgressOptionPanel { get { return progressOptionPanel; } }
    public QuestRobberEventPanel RobberEventPanel { get { return robberEventPanel; } }
    public QuestDirectingPanel QuestDirectingPanel { get { return questDirectingPanel; } }
    public float Toll { get { return toll; } }

    public Queue<Pyosa> GetawayEscort { get { return getawayEscort; } set { getawayEscort = value; } }
    public Queue<Pyosa> SeriousInjuryEscort { get { return seriousInjuryEscort; } set { seriousInjuryEscort = value; } }
    public Queue<Pyosa> SlightInjuryEscort { get { return slightInjuryEscort; } set { slightInjuryEscort = value; } }
    public Queue<Pyosa> DeathEscort { get { return deathEscort; } set { deathEscort = value; } }

    #endregion

    private void OnDestroy()
    {
        if (targetQuest != null)
        {
            targetQuest.onNewRouteGroup -= UpdateQuestDescriptors;
            targetQuest.onCompleted -= DestroySelf;
        }
        foreach (var tuple in questDesciptorByRoute)
        {
            var route = tuple.Key;
            route.onValueChanged -= UpdateProgressBar;
        }
    }
    public void Report()
    {
        QuestSystem.Instance.ReceiveReport(targetQuest.QuestId, targetQuest.CurrentRoute.Dest, 1);
    }
    public void Setup(Quest targetQuest, QuestProgressOptionPanel optionPanel, QuestRobberEventPanel robberPanel, QuestDirectingPanel directingPane)
    {
        trackerBtn = GetComponent<Button>();
        trackerBtn.interactable = false;
        trackerBtn.onClick.AddListener(() => OnTrackerBtn());

        progressOptionPanel = optionPanel;
        questDirectingPanel = directingPane;
        robberEventPanel = robberPanel;

        this.targetQuest = targetQuest;

        CountryDate.Instance.OnReport += Report;

        maxValue = targetQuest.Deadline;
        currentMaxValue = maxValue;
        this.currentValue = minValue;

        targetQuest.onNewRouteGroup += UpdateQuestDescriptors;

        targetQuest.onCompleted += DestroySelf;

        var routes = targetQuest.Routes;
        CreateQuestDescriptor();
        UpdateQuestDescriptors(targetQuest, routes[0]);

    }

    public void UpdateQuestDate(int value)
    {
        currentMaxValue -= value;

        Debug.Log("Update quest date");
    }

    void CreateQuestDescriptor()
    {
        questDescriptor = Instantiate(questDescriptorPrefab, transform);
    }
    void UpdateQuestDescriptors(Quest quest, Route currentRoute)
    {
        //questReporter.Setup(quest.QuestId, currentRoute.Dest, this);
        questDescriptor.UpdateCurrentPath(quest, currentRoute);

        Debug.Log($"QuestTracker - currentRoute : {currentRoute.Dest}");

        currentRoute.onValueChanged += UpdateProgressBar;
        questDesciptorByRoute.Add(currentRoute, questDescriptor);

        if (currentRoute.Dest == Dest.Returning)
        {
            maxValue = quest.Deadline / 2;
            currentValue = minValue;
        }
        if(quest.Pyodu.IsPlayer)
        {
            if (quest.QuestType == QuestType.Transport || quest.QuestType == QuestType.Guard)
            {
                if (currentRoute.Dest != Dest.Returning)
                {
                    //메인화면 UI 입력 제한
                    Manager.ins.city_MainUI.SetInteractable(false);
                    Manager.ins.questEventManager.EnqueueEvent(QuestEventType.Progress);
                    Manager.ins.questEventManager.StartEvent(this, currentRoute);
                }
                else
                {
                    //메인화면 UI 입력 제한
                    Manager.ins.city_MainUI.SetInteractable(true);
                }
            }
        }
    }
    void UpdateProgressBar(Route route, int currentValue, int prevValue)
    {
        this.currentValue++;
        progressBarImage.fillAmount = this.currentValue / maxValue;

        if (route.Dest != Dest.Returning)
            CheckRobberEvent(route);

        if(currentMaxValue == this.currentValue && route.Dest != Dest.Returning)
        {
            targetQuest.ForceToReturn();
        }
        if (maxValue == this.currentValue)
        {
            OnComplete();
        }
    }

    void CheckRobberEvent(Route route)
    {
        float randomValue = Random.value * 100.0f;

        //도적 이벤트가 발생할 경우
        if (randomValue < Manager.ins.questEventManager.RobberEvent.EncountProbability)
        {
            int locationIndex = (int)route.Dest;

            if (QuestSystem.Instance.GetRobber((Province)locationIndex) == null)
                return;
            else
            {
                if(Manager.ins.questEventManager.IsEventActive)
                {
                    Manager.ins.questEventManager.EnqueueEvent(QuestEventType.Robber);
                    Debug.Log("다른 이벤트 진행중 - 큐에 추가 합니다.");
                }
                else
                {
                    Debug.Log("진행 중인 이벤트 없음 - 이벤트를 실행합니다.");
                    Manager.ins.questEventManager.EnqueueEvent(QuestEventType.Robber);
                    Manager.ins.questEventManager.StartEvent(this, route);
                }
            }
        }
        else return;
    }


    void OnComplete()
    {
        //questReporter.StopReport();
        CountryDate.Instance.OnReport -= Report;
        questDescriptor.OnComplete();
        questDirectingPanel.gameObject.SetActive(false);

        trackerBtn.interactable = true;

    }

    void OnTrackerBtn()
    {
        if(completePanel.gameObject.activeSelf)
        {
            completePanel.gameObject.SetActive(false);
        }
        else
        {
            completePanel.gameObject.SetActive(true);
            completePanel.Setup(this);
        }

    }

    private void DestroySelf(Quest quest)
    {
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestEventType
{
    Progress,
    Robber,
}
public class QuestEventManager : MonoBehaviour
{
    //인스펙터 셋팅
    [Header("Panels")]
    [SerializeField] QuestDirectingPanel directingPanel;
    [SerializeField] QuestProgressOptionPanel progressOptionPanel;
    [SerializeField] QuestRobberEventPanel robberEventPanel;

    //코드 셋팅
    [Header("Events")]
    [SerializeField] ProgressEvent progressEvent;
    [SerializeField] RobberEvent robberEvent;
    //코드 셋팅
    [Space(10)]
    [SerializeField] QuestTracker questTracker;

    Queue<QuestEventType> eventQueue = new Queue<QuestEventType>();

    [SerializeField]
    bool isEventActive = false;

    #region Properties
    public QuestTracker QuestTracker => questTracker;
    public QuestDirectingPanel DirectingPanel => directingPanel;
    public RobberEvent RobberEvent => robberEvent;
    public ProgressEvent ProgressEvent => progressEvent;
    public bool IsEventActive => isEventActive;

    #endregion

    private void Awake()
    {
        robberEvent = GetComponent<RobberEvent>();
        progressEvent = GetComponent<ProgressEvent>();
    }
    public void EnqueueEvent(QuestEventType eventType)
    {
        eventQueue.Enqueue(eventType);
    }
    public void StartEvent(QuestTracker questTracker, Route currnetRoute)
    {
        if (!questTracker.TargetQuest.Pyodu.IsPlayer)
            return;

        this.questTracker = questTracker;
        CountryDate.Instance.TimeFlowingSet(false);
        //this.reporter.StopReport();
       

        if (eventQueue.Count > 0 && !isEventActive)
        {
            isEventActive = true;

            QuestEventType currnetEventType = eventQueue.Dequeue();

            if (currnetEventType == QuestEventType.Progress)
            {
                progressOptionPanel.gameObject.SetActive(true);
                progressOptionPanel.Show(questTracker,currnetRoute.Dest);
            }
            else if (currnetEventType == QuestEventType.Robber)
            {
                int locationIndex = (int)currnetRoute.Dest;

                robberEventPanel.gameObject.SetActive(true);
                robberEventPanel.Setup(questTracker, QuestSystem.Instance.GetRobber((Province)locationIndex));
            }
        }

    }

    public void EndEvent()
    {
        isEventActive = false;

        if (eventQueue.Count > 0 && questTracker.TargetQuest.CurrentRoute.Dest != Dest.Returning)
        {
            StartEvent(questTracker, questTracker.TargetQuest.CurrentRoute);
        }
        else
        {
            eventQueue.Clear();
            //reporter.StartReport();
            CountryDate.Instance.TimeFlowingSet(true);
        }
    }

    public void QueueNextEvent(QuestEventType eventType)
    {
        eventQueue.Enqueue(eventType);
    }
}

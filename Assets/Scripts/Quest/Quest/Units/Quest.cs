using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForCompletion,
}

public class Quest : MonoBehaviour
{
    #region Events
    public delegate void RouteValueChangedHandler(Quest quest, Route route, int current, int prev);
    public delegate void RegisteredHandler(Quest quest);
    public delegate void CompletedHandler(Quest quest);
    //public delegate void CanceledHandler(Quest quest);
    public delegate void NewRouteGroupHandler(Quest quest, Route currentRoute);

    public event RouteValueChangedHandler onRouteValueChanged;
    public event RegisteredHandler onRegistered;
    public event CompletedHandler onCompleted;
    //public event CanceledHandler onCanceled;
    public event NewRouteGroupHandler onNewRouteGroup;
    #endregion

    [Header("Visible Info")]
    [SerializeField] Text typeText;
    [SerializeField] Text ownerNameText;
    [SerializeField] Text contentText;
    [SerializeField] Text destText;
    [SerializeField] Text deadineText;
    [SerializeField] Text rewardText;

    [SerializeField] int questId;
    [SerializeField] string ownerName;
    [SerializeField] string questDesc;
    [SerializeField] int deadline;
    [SerializeField] int reward;

    [SerializeField] Pyosa pyodu;
    [SerializeField] List<Pyosa> attendedEscortList = new List<Pyosa>();

    public QuestUnit questUnit;
    public Route[] routes;
    private int currentRouteIndex = 0;
    private Toggle toggle;
   

    [SerializeField]
    QuestState state;
    QuestType questType;
    OwnerType ownerType;
    QuestLevel questLevel;
    Dest dest;

    #region Properties
    public int QuestId { get { return questId; } set { questId = value; } }
    public string OwnerName { get { return ownerName; } set { ownerName = value; } }
    public string QuestDesc { get { return questDesc; } set { questDesc = value; } }
    public int Deadline { get { return deadline; } set { deadline = value; } }
    public int Reward { get { return reward; } set { reward = value; } }

    public Pyosa Pyodu { get { return pyodu; } set { pyodu = value; } }
    public List<Pyosa> AttendedEscortList { get { return attendedEscortList; } set { attendedEscortList = value; } }
    public QuestType QuestType { get { return questType; } set { questType = value; } }
    public OwnerType OwnerType { get { return ownerType; } set { ownerType = value; } }
    public Dest Dest { get { return dest; } set { dest = value; } }
    public QuestLevel QuestLevel { get { return questLevel; } set { questLevel = value; } }
    public Route[] Routes { get { return routes; } set { routes = value; } }
    public Route CurrentRoute => Routes[currentRouteIndex];

    public QuestState State { get { return state ; } set { state = value; } }

    public bool IsAvailable => State == QuestState.Inactive;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCompleteable => State == QuestState.WaitingForCompletion;

    #endregion

    public void Setup()
    {
        typeText.text = $"{QuestType}";
        ownerNameText.text = OwnerName;
        contentText.text = QuestDesc;
        destText.text = $"{dest}";
        deadineText.text = $"{Deadline}";
        rewardText.text = $"{Reward}";

        toggle = gameObject.GetComponent<Toggle>();

        SetRoute();
    }

    private void SetRoute()
    {
        var routesLength = questUnit.GetType().GetFields().Where(field => field.Name.Contains("Route"))
            .Select(field => field.GetValue(questUnit)).Where(value => value != null && !value.Equals(0)).Count();
        routes = new Route[routesLength+1];

        var dests = questUnit.GetType().GetFields().Where(field => field.Name.Contains("Route"))
          .Select(field => field.GetValue(questUnit)).Where(value => value != null && !value.Equals(0)).ToArray();

        var dates = questUnit.GetType().GetFields().Where(field => field.Name.Contains("Date"))
            .Select(field => field.GetValue(questUnit)).Where(value => value != null && !value.Equals(0)).ToArray();

        for (int i = 0; i < dests.Length; i++)
        {
            Route route = new Route();

            route.Dest = (Dest)dests[i];
            route.Date = (int)dates[i];

            routes[i] = route;
        }

       Route returnRoute = new Route();
        returnRoute.Dest = Dest.Returning;
        returnRoute.Date = Mathf.RoundToInt(deadline / 2);

        routes[dests.Length] = returnRoute;

    }

    public void OnAvailable()
    {
        Debug.Assert(!IsAvailable, "This quest has already been on the available questList");
        State = QuestState.Inactive;
    }
    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "This quest has already been registered");

        gameObject.SetActive(false);

        foreach (var route in routes)
        {
            route.Setup(this);
            route.onValueChanged += OnValueChanged;
        }
        onRegistered?.Invoke(this);
        State = QuestState.Running;
        routes[currentRouteIndex].OnRouteStart();

    }

    public void ReceiveReport(int questId, Dest dest, int value)
    {
        Debug.Assert(IsRegistered, "This questTest has already been registered");

        if (IsComplete)
            return;

        CurrentRoute.ReceiveReport(questId,value);

        if (CurrentRoute.IsComplete)
        {
            if (currentRouteIndex + 1 == routes.Length)
            {
                State = QuestState.WaitingForCompletion;
            }
            else
            {
                var prevRoute = routes[currentRouteIndex++];
                prevRoute.OnRouteEnd();
                CurrentRoute.OnRouteStart();
                onNewRouteGroup?.Invoke(this, CurrentRoute);
            }
        }
        else
        {
            State = QuestState.Running;
        }
    }

    public void ForceToReturn()
    {
        CheckingIsRunning();

        for (int i = 0; i < routes.Length-1; i++)
        {
            routes[i].Complete();
        }
        currentRouteIndex = routes.Length-1;

        CurrentRoute.OnRouteEnd();
        onNewRouteGroup?.Invoke(this, CurrentRoute);

        Debug.Log("ForceToReturn");
    }
    public void Complete()
    {
        CheckingIsRunning();

        foreach(var route in routes)
            route.Complete();

        State = QuestState.Complete;

        onCompleted?.Invoke(this);

        onRouteValueChanged = null;
        onCompleted = null;
        //onCanceled = null;
        onNewRouteGroup = null;
    }
    private void OnValueChanged(Route route, int currentValue, int prevValue)
        => onRouteValueChanged?.Invoke(this, route, currentValue, prevValue);

    [Conditional("UNITY_EDITOR")] //인자로 전달한 Simbol값이 선언되어있으면 함수를 실행하고 아니라면 함수를 무시하게 해주는 Attribute
    private void CheckingIsRunning()
    {
        Debug.Assert(IsRegistered, "This questTest has already been registered");
        Debug.Assert(!IsComplete, "This questTest has been completed");
    }
}

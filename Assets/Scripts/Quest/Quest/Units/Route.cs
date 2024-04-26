using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RouteState
{
    Inactive,
    Running,
    Complete,
}
[System.Serializable]
public class Route
{
    #region Events
    public delegate void StateChangedHandler(Route route, RouteState currentState, RouteState prevState);
    public delegate void ValueChangedHandler(Route route, int currentValue, int prevValue);

    public event StateChangedHandler onStateChanged;
    public event ValueChangedHandler onValueChanged;
    #endregion

    [SerializeField] private RouteState state;
    private int initialValue = 0;
    [SerializeField] private int currentValue;

    [SerializeField] private Dest dest;
    [SerializeField] private int date;

    #region Properties

    public int CurrentValue
    {
        get => currentValue;
        set
        {
            int prevValue = currentValue;
            currentValue = Mathf.Clamp(value, 0, date);
            if (currentValue != prevValue)
            {
                state = currentValue == date ? RouteState.Complete : RouteState.Running;
                onValueChanged?.Invoke(this, currentValue, prevValue);
            }
        }
    }
    public RouteState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            onStateChanged?.Invoke(this, state, prevState);
        }
    }

    public Dest Dest { get => dest; set => dest = value; }
    public int Date { get => date; set => date = value; }

    public bool IsComplete => State == RouteState.Complete;

    public Quest Owner { get; private set; }
    public void Setup(Quest owner)
    {
        Owner = owner;
    }
    #endregion

    public void OnRouteStart()
    {
        State = RouteState.Running;
        CurrentValue = initialValue;
    }
    public void OnRouteEnd()
    {
        onStateChanged = null;
        onValueChanged = null;
    }
   
    public void ReceiveReport(int questId,int value)
    {
        if(Owner.QuestId == questId)
        {
            CurrentValue = currentValue + value;
        }
    }
    public void Complete()
    {
        CurrentValue = date;
    }
}

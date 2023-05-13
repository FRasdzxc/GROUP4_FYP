using UnityEngine;
using System;

public class EventRequestData : ScriptableObject
{
    protected Action actions;

    public virtual void Invoke()
        => actions?.Invoke();
        
    public virtual void AddEvent(Action action)
        => actions += action;
    public virtual void RemoveAllEvents()
        => actions = null;
}

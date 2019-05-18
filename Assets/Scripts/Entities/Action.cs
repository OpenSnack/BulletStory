using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class Action : IActionHolder
{
    public float time;
    protected List<Action> actions;

    public abstract void Act(Enemy source, ActionContext context);

    public void AddAction(Action action)
    {
        if (actions == null)
        {
            actions = new List<Action>();
        }

        actions.Add(action);
        actions.OrderBy(a => a.GetTime()); // might be slow
    }

    public float GetTime()
    {
        return time;
    }
}

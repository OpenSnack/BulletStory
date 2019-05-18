using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Loop : Action
{
    public float period;
    private bool notFirstLoop;

    public Loop(float tm, float prd, List<Action> actns)
    {
        time = tm;
        period = prd;
        actions = actns;
        notFirstLoop = true;
    }

    public override void Act(Enemy source, ActionContext context)
    {
        foreach (Action action in actions)
        {
            if (notFirstLoop)
            {
                action.time += period;
            }
            source.AddAction(action);
        }

        Loop nextIteration = new Loop(time + period, period, actions);
        source.AddAction(nextIteration);
    }
}

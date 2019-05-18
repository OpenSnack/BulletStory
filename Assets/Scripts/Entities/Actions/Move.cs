using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Move : Action
{
    public float duration;
    public float destX;
    public float destY;
    public string ease;

    public Move(float tm, float dur, float dstX, float dstY, string eas)
    {
        time = tm;
        duration = dur;
        destX = dstX;
        destY = dstY;
        ease = eas;
    }

    public override void Act(Enemy source, ActionContext context)
    {
        source.Move(destX, destY, duration, ease);
    }
}

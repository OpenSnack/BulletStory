using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveForever : Action
{
    public float speed;

    public MoveForever(float tm, float spd)
    {
        time = tm;
        speed = spd;
    }

    public override void Act(Enemy source, ActionContext context)
    {
        source.MoveForever(speed);
    }
}

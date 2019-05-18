using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Homing : Action
{
    public float speed;
    public string bullet;

    public override void Act(Enemy source, ActionContext context)
    {
        // instantiate bullet with single action
        if (bullet == null)
        {
            bullet = "Diamond";
        }
        GameObject obj = context.allEnemies[bullet];

        Vector2 from = source.transform.position;
        Vector2 to = context.player.transform.position;
        float angle = Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject homer = GameObject.Instantiate(obj, source.transform.position, rotation, context.bulletContainer);
        homer.GetComponent<Enemy>().AddAction(new MoveForever(0, speed / 100));
    }
}

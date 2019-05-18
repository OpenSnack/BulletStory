using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Burst : Action
{
    public float speed;
    public int n;
    public float rotate;
    public string bullet;

    public override void Act(Enemy source, ActionContext context)
    {
        if (bullet == null)
        {
            bullet = "Diamond";
        }
        GameObject obj = context.allEnemies[bullet];

        Vector2 from = source.transform.position;

        for (int i = 0; i < n; i++)
        {
            float angle = -90 + rotate + i * 360 / n;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject homer = GameObject.Instantiate(obj, source.transform.position, rotation, context.bulletContainer);
            homer.GetComponent<Enemy>().AddAction(new MoveForever(0, speed / 100));
        }
    }
}

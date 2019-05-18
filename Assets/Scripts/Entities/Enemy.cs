using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IActionHolder
{
    public int health;
    private float time = 0f;
    private List<Action> actions;
    private bool paused;
    private ActionContext context;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetContext(ActionContext ctx)
    {
        context = ctx;
    }

    public void AddAction(Action action)
    {
        if (actions == null)
        {
            actions = new List<Action>();
        }

        actions.Add(action);
        actions.OrderBy(a => a.GetTime()); // might be slow
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            time += Time.deltaTime;
            while (actions.Any() && actions[0].GetTime() <= time)
            {
                // do action
                actions[0].Act(this, context);
                actions.RemoveAt(0);
            }
        }
    }

    public void Move(float toX, float toY, float duration, string ease)
    {
        StartCoroutine(MoveRoutine(toX, toY, duration, ease));
    }

    private IEnumerator MoveRoutine(float toX, float toY, float duration, string ease)
    {
        float time = 0f;
        Vector2 startPosition = transform.position;
        Vector2 to = Spawner.ScaleToAspectRatio(toX, toY);

        while (time < duration)
        {
            if (!paused)
            {
                time += Time.deltaTime;

                Vector2 newPos;
                switch (ease)
                {
                    case "InOut":
                        newPos = Mathfx.Hermite(startPosition, to, time / duration);
                        break;
                    case "Out":
                        newPos = Mathfx.Sinerp(startPosition, to, time / duration);
                        break;
                    case "In":
                        newPos = Mathfx.Coserp(startPosition, to, time / duration);
                        break;
                    default:
                        newPos = Vector2.Lerp(startPosition, to, time / duration);
                        break;
                }
                transform.position = newPos;
            }

            yield return null;
        }
    }

    public void MoveForever(float speed)
    {
        StartCoroutine(MoveForeverRoutine(speed));
    }

    private IEnumerator MoveForeverRoutine(float speed)
    {
        while (true)
        {
            if (!paused)
            {
                float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                float addX = speed * Mathf.Cos(angle);
                float addY = speed * Mathf.Sin(angle);
                transform.Translate(new Vector2(addX, addY), Space.World);
            }

            yield return null;
        }
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }
}

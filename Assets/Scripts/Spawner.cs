using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour, LevelSection
{
    private const int MAX_HEIGHT = 100;
    private const int MAX_WIDTH = 100;

    private Dictionary<string, GameObject> allEnemies;
    private Dictionary<string, Type> allActions;
    public Queue<JSONNode> enemies;
    public List<GameObject> spawned;
    private float time;
    private bool paused = false;

    private ActionContext context;

    // Start is called before the first frame update
    void Start()
    {
        allActions = new Dictionary<string, Type>()
        {
            { "Move", typeof(Move) },
            { "Homing", typeof(Homing) },
            { "Burst", typeof(Burst) },
            { "Loop", typeof(Loop) }
        };
        spawned = new List<GameObject>();
        time = 0f;
        
        context = new ActionContext();
        context.player = GameObject.FindWithTag("player");
        context.enemyContainer = transform.Find("Enemies");
        context.bulletContainer = transform.Find("Bullets");
        context.allEnemies = allEnemies;
        context.spawner = this;
    }

    public void SetAllEnemies(Dictionary<string, GameObject> all)
    {
        allEnemies = all;
    }

    public void SetData(List<JSONNode> spec)
    {
        enemies = new Queue<JSONNode>(spec.OrderBy(enemy => enemy["time"].AsFloat));
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && enemies != null)
        {
            if (enemies.Any() || spawned.Any(enemy => enemy != null))
            {
                time += Time.deltaTime;
                while (enemies.Any() && enemies.Peek()["time"].AsFloat <= time)
                {
                    // spawn next enemy
                    Spawn(enemies.Dequeue());
                }
            }
        }
    }

    private void Spawn(JSONNode next)
    {
        Vector2 position = ScaleToAspectRatio(next["dest"][0].AsFloat, next["dest"][1].AsFloat);
        GameObject enemy = Instantiate(allEnemies[next["type"].Value], position, transform.rotation, context.enemyContainer);
        Enemy component = enemy.GetComponent<Enemy>();
        component.SetContext(context);

        AddActions(next, component);

        spawned.Add(enemy);
    }

    public void AddActions(JSONNode next, IActionHolder ah)
    {
        foreach (JSONNode actionSpec in next["actions"].Children)
        {
            string jsonString = actionSpec.ToString();
            if (allActions.ContainsKey(actionSpec["type"].Value))
            {
                Type t = allActions[actionSpec["type"].Value];
                Action action = JsonUtility.FromJson(jsonString, t) as Action;

                if (actionSpec.HasKey("actions"))
                {
                    AddActions(actionSpec, action);
                }

                ah.AddAction(action);
            }
        }
    }

    public void Add(GameObject enemy)
    {
        spawned.Add(enemy);
    }

    public static Vector2 ScaleToAspectRatio(float x, float y)
    {
        float size = Camera.main.orthographicSize;
        float newX = size * ((x * 2 / MAX_WIDTH) - 1);
        float newY = (size * Camera.main.aspect) * (1 - (y * 2 / MAX_HEIGHT));
        return new Vector2(newX, newY);
    }

    public void Pause()
    {
        enabled = false;
    }

    public void Unpause()
    {
        enabled = true;
    }

    public bool IsCompleted()
    {
        return enemies != null && !enemies.Any() && spawned.All(item => item == null);
    }
}

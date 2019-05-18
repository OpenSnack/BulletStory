using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContext
{
    public GameObject player;
    public Spawner spawner;
    public Transform enemyContainer;
    public Transform bulletContainer;
    public Dictionary<string, GameObject> allEnemies;
}

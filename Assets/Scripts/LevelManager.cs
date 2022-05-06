using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;
using System.Linq;
using Yarn.Unity;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    public TextAsset levelListingAsset;
    public DialogueRunner dialogueRunner;

    private Dictionary<string, GameObject> allEnemies;
    private JSONNode levelListing;
    private JSONNode levelJSON;
    private Queue<LevelSection> sections;
    private bool paused;

    private Transform enemyContainer;
    private Transform bulletContainer;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        enemyContainer = transform.Find("Enemies");
        bulletContainer = transform.Find("Bullets");

        sections = new Queue<LevelSection>();

        levelListing = JSON.Parse(levelListingAsset.text);
        // replace this eventually with the result of, like, a save file's progress or something
        var firstLevel = levelListing["levels"].Children
            .FirstOrDefault(level => level["id"].Value == levelListing["tree"]["id"].Value)["path"].Value;
        var path = $"Levels/{firstLevel}";
        var properPathName = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        LoadLevel(properPathName);

        RunLevel();
    }

    public void LoadLevel(string filename)
    {
        sections = new Queue<LevelSection>();

        var levelTextAsset = Resources.Load<TextAsset>(filename);
        levelJSON = JSON.Parse(levelTextAsset.text);
        foreach (JSONNode section in levelJSON)
        {
            switch (section["section"].Value)
            {
                case "bullet":
                    MakeSpawner(section);
                    break;
                case "story":
                    MakeStory(section);
                    break;
            }
        }
    }

    public void MakeSpawner(JSONNode section)
    {
        if (allEnemies == null)
        {
            allEnemies = Resources.LoadAll<GameObject>("Prefabs")
                .ToList()
                .ToDictionary(obj => obj.name);
        }

        List<JSONNode> enemies = section["enemies"].Children.ToList();
        Spawner spawner = gameObject.AddComponent<Spawner>();
        spawner.SetAllEnemies(allEnemies);
        spawner.SetData(enemies);
        sections.Enqueue(spawner);
    }

    public void MakeStory(JSONNode section)
    {

    }

    public void RunLevel()
    {
        Unpause();
    }

    public void Pause()
    {
        if (sections.Count() > 0)
        {
            sections.Peek().Pause();
        }

        foreach (Transform child in enemyContainer)
        {
            child.GetComponent<Enemy>().Pause();
        }

        foreach (Transform child in bulletContainer)
        {
            child.GetComponent<Enemy>().Pause();
        }
        paused = true;
    }

    public void Unpause()
    {
        if (sections.Count() > 0)
        {
            sections.Peek().Unpause();
        }

        foreach (Transform child in enemyContainer)
        {
            child.GetComponent<Enemy>().Unpause();
        }

        foreach (Transform child in bulletContainer)
        {
            child.GetComponent<Enemy>().Unpause();
        }
        paused = false;
    }

    public bool GetPaused()
    {
        return paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (sections.Count() > 0 && sections.Peek().IsCompleted())
        {
            sections.Dequeue();
            if (sections.Any())
            {
                Unpause(); // or something
            }
            else
            {
                // level complete
                Pause(); // or something
            }
        }
    }
}

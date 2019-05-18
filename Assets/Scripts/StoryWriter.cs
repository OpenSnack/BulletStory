using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryWriter : MonoBehaviour, LevelSection
{
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetData(List<JSONNode> spec)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
        return false;
    }
}

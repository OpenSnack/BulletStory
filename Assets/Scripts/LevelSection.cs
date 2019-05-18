using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LevelSection
{
    void SetData(List<JSONNode> spec);
    void Pause();
    void Unpause();
    bool IsCompleted();
}

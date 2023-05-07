using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation Data", menuName = "Scriptable Object/Conversation Data")]
public class ConversationData : ScriptableObject
{
    [SerializeField] private Dictionary<string, Script> scriptDict;
}

[Serializable]
public class Script
{
    public string name;
    public string script;
}

[Serializable]
public class Content
{
    public string title;
    public List<Script> contents = new List<Script>();
}

[Serializable]
public class ScriptLoader : ILoader<string, List<Script>>
{
    public List<Content> scripts = new List<Content>();

    public Dictionary<string, List<Script>> MakeDict()
    {
        Dictionary<string, List<Script>> dic = new Dictionary<string, List<Script>>();
        foreach (Content content in scripts)
        {
            dic.Add(content.title, content.contents);
        }
        return dic;
    }
}
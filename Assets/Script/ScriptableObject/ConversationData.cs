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
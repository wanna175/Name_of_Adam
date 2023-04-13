using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/Data")]
public class GameData : ScriptableObject
{
    public int Money;
    public int DarkEssence;
    public List<DeckUnit> decks = new List<DeckUnit>();
}
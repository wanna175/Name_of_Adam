using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/Data")]
public class GameData : ScriptableObject
{
    public int Money;
    public int DarkEssence;
    public List<DeckUnit> DeckUnits = new List<DeckUnit>();
    public bool isVisitUpgrade = false;
}
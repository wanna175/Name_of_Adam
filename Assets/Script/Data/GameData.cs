using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/Data")]
public class GameData : ScriptableObject
{
    public Incarna Incarna;
    public int Money;
    public int DarkEssence;
    public int PlayerSkillCount;
    public List<DeckUnit> DeckUnits = new();
    public bool isVisitUpgrade = false;
    public bool isVisitStigma = false;
}
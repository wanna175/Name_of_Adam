using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/Data")]
public class GameData : ScriptableObject
{
    public Incarna Incarna;
    public int DarkEssence;
    public int PlayerHP;
    public List<DeckUnit> DeckUnits = new();
    public List<DeckUnit> FallenUnits = new();
    public bool IsVisitUpgrade = false;
    public bool IsVisitStigma = false;
    public bool IsVisitDarkShop = false;
    public Progress Progress;
    public Vector3 StageDivine;
}

[Serializable]
public class Progress
{
    public int NormalWin;
    public int EliteWin;
    public int BossWin;
    public int NormalKill;
    public int EliteKill;
    public int PhanuelKill;
    public int HorusKill;
    public int FishKill;
    public int NormalFall;
    public int EliteFall;
    public int PhanuelFall;
    public int HorusFall;
    public int FishFall;
    public int SecChapterClear;
    public int LeftDarkEssence;
    public int SurvivedNormal;
    public int SurvivedElite;
    public int SurvivedBoss;

    public void ClearProgress()
    {
        NormalWin = 0;
        EliteWin = 0;
        BossWin = 0;
        NormalKill = 0;
        EliteKill = 0;
        PhanuelKill = 0;
        HorusKill = 0;
        FishKill = 0;
        NormalFall = 0;
        EliteFall = 0;
        PhanuelFall = 0;
        HorusFall = 0;
        FishFall = 0;
        SecChapterClear = 0;
    }
}

[Serializable]
public class NPCQuest
{
    public int UpgradeQuest;
    public int StigmaQuest;
    public int DarkshopQuest;
    public void ClearQuest()
    {
        UpgradeQuest = 0;//Á×ÀÎ Àû È½¼ö
        StigmaQuest = 0;//³«ÀÎ ºÎ¿©È½¼ö
        DarkshopQuest = 0;//Àû Å¸¶ôÈ½¼ö
    }
}
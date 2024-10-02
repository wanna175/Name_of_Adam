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
    public Progress Progress;
    public Vector3 StageDivine;
    public int CurrentAct;
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
    public int SaviorKill;
    public int YohrnKill;
    public int NormalFall;
    public int EliteFall;
    public int PhanuelFall;
    public int SaviorFall;
    public int YohrnFall;
    public int AllChapterClear;
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
        SaviorKill = 0;
        YohrnKill = 0;
        NormalFall = 0;
        EliteFall = 0;
        PhanuelFall = 0;
        SaviorFall = 0;
        YohrnFall = 0;
        AllChapterClear = 0;
    }
}

//제거된 값, 업데이트를 위해서만 이용됨
[Serializable]
public class NPCQuest
{
    public int UpgradeQuest;
    public int StigmaQuest;
    public int DarkshopQuest;
    public void ClearQuest()
    {
        UpgradeQuest = 0;//죽인 적 횟수
        StigmaQuest = 0;//낙인 부여횟수
        DarkshopQuest = 0;//적 타락횟수
    }
}
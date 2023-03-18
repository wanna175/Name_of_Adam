using System;
using System.Collections.Generic;
using UnityEngine;


// 인스펙터에서 스테이지 정보를 받기 위한 테스트 클래스
[Serializable]
public struct TestContainer
{
    public string Name;
    public string Type;
    public int MaxCount;
    public int MaxAppear;
}


// 현재 전투 스테이지의 경우 팩션에 대한 데이터가 없음
// Type == Battle 인 스테이지에 팩션에 대한 데이터가 추가되어야 함

public class Stage
{
    // 담고있는 정보가 어떤 스테이지의 것인지 확인하기 위한 변수
    public string Name;
    string Type;     // 전투, 이벤트 등 
    public Faction Faction;
    int MaxAppear;   // 필드에 출현 가능한 최대 갯수
    int NowAppear;
    int MaxCount;    // 최대 출현 가능 갯수
    int RemainCount;

    public Stage(string name, string type, int count, int appear, Faction f = 0)
    {
        Name = name;
        Type = type;
        faction = f;
        MaxAppear = appear;
        NowAppear = 0;
        MaxCount = RemainCount = count;
    }

    public string GetStageType() => Type;
    public int GetRemainCount() => RemainCount;

    // 이 스테이지가 출현 가능한 조건이라면 true를 반환
    public bool GetStage()
    {
        // 최대 출현 가능 갯수 초과
        if (MaxAppear <= NowAppear)
            return false;

        // 카운트 초과
        if (RemainCount <= 0)
            return false;

        RemainCount--;
        NowAppear++;
        return true;
    }

    public void RecallCount()
    {
        if (RemainCount <= MaxCount)
            RemainCount++;
    }

    public void InitCount()
    {
        RemainCount = MaxCount;
    }

    // 현재 클래스의 내용물을 복사하여 새로운 객체를 반환
    public Stage Clone() => new Stage(Name, Type, MaxCount, MaxAppear);
    public void AppearClear() => NowAppear = 0;
}
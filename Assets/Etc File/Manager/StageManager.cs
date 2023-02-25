using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    // StageName은 입력받는 방식이 달라지면 사라질 변수
    public string StageName;
    int MaxAppear;
    int MaxCount;
    int RemainCount;

    public StageInfo(int appear, int count)
    {
        MaxAppear = appear;
        MaxCount = RemainCount = count;
    }

    public bool GetStage(int AppearCount)
    {
        if(AppearCount >= MaxAppear)
            return false;
        if (RemainCount >= 0)
            return false;

        RemainCount--;
        return true;
    }

    public void RecallCount()
    {
        if(RemainCount <= MaxCount)
            RemainCount++;
    }
}

public class StageManager : MonoBehaviour
{
    List<string> MapList = new List<string>();
    Dictionary<string, StageInfo> StageDict = new Dictionary<string, StageInfo>();

    string[] NextStageArray = new string[3];
    string[] AfterNextStageArray = new string[5];

    // 인스펙터에서 스테이지 정보를 받기 위해 만든 테스트용 리스트
    [SerializeField] List<StageInfo> StageInfoList;
    

    void InitStage()
    {
        MapList.Clear();
        StageDict.Clear();

        SetStageData();
        SetMapList();

        for(int i = 0; i < NextStageArray.Length; i++)
            NextStageArray[i] = MapList[0];
        SetAfterArray();
    }

    // 인스펙터에서 받은 데이터를 입력하는 용도로 만듦
    // 데이터를 받는 방식이 달라지면 수정할 메서드
    void SetStageData()
    {
        foreach(StageInfo si in StageInfoList)
            StageDict.Add(si.StageName, si);
    }

    void SetMapList()
    {
        int randStageCount = 2;

        for(int i = 4; i > 0; i--)
        {
            MapList.Add("전투");
            MapList.Add("랜덤");

            if (i <= randStageCount) {
                MapList.Add("랜덤");
                randStageCount--;

                continue;
            }

            if (Random.Range(0, 1) == 0)
            {
                MapList.Add("랜덤");
                randStageCount--;
            }
        }
    }


    void SetAfterArray()
    {

    }

    string GetRandomStage()
    {
        string stage = null;

        return stage;
    }
}
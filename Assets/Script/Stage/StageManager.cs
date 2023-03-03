using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct TestContainer
{
    public string StageName;
    public int MaxCount;
    public int MaxAppear;
}

public class Stage
{
    // 담고있는 정보가 어떤 스테이지의 것인지 확인하기 위한 변수
    public string StageName;
    int MaxAppear;
    int NowAppear;
    int MaxCount;
    int RemainCount;

    public Stage(int count, int appear)
    {
        MaxAppear = appear;
        NowAppear = 0;
        MaxCount = RemainCount = count;
    }

    public int GetRemainCount() => RemainCount;

    public bool GetStage()
    {
        // 최대 출현 가능 갯수 초과
        if (MaxAppear <= NowAppear)
        {
            return false;
        }
        // 카운트 초과
        if (RemainCount <= 0)
        {
            return false;
        }

        RemainCount--;
        NowAppear++;
        return true;
    }

    public void RecallCount()
    {
        if (RemainCount <= MaxCount)
        {
            RemainCount++;
            NowAppear--;
        }
    }
}

public class StageManager : MonoBehaviour
{
    List<string> MapList = new List<string>();
    Dictionary<string, Stage> StageDict = new Dictionary<string, Stage>();

    // 디버그용 시리얼라이즈
    [SerializeField] string[] NextStageArray = new string[3];
    [SerializeField] string[] AfterNextStageArray = new string[5];

    // 인스펙터에서 스테이지 정보를 받기 위해 만든 테스트용 리스트
    // 얘는 데이터 매니저에서 받아야하려나?
    // 일단은 게임매니저 아래에 StageManger를 생성하고 인스펙터에서 받아오는 식으로 해보자
    [SerializeField] public List<TestContainer> StageInfoContainer;
    
    List<Stage> StageInfoList;


    private void Start()
    {
        StageInfoList = GetInfoList();

        InitStage();
    }

    public List<Stage> GetInfoList()
    {
        List<Stage> stageList = new List<Stage>();

        foreach (TestContainer test in StageInfoContainer)
        {
            Stage st = new Stage(test.MaxCount, test.MaxAppear);
            st.StageName = test.StageName;

            stageList.Add(st);
        }

        return stageList;
    }


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
        foreach(Stage si in StageInfoList)
            StageDict.Add(si.StageName, si);
    }

    void SetMapList()
    {
        int randStageCount = 2;

        for(int i = 4; i > 0; i--)
        {
            if (i == 2)
                MapList.Add("엘리트");
            else
                MapList.Add("전투");

            MapList.Add("랜덤");

            if (randStageCount <= 0)
                continue;

            if (i <= randStageCount) {
                MapList.Add("랜덤");
                randStageCount--;
                Debug.Log(i + "번째에 랜덤 추가");
                continue;
            }

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                MapList.Add("랜덤");
                randStageCount--;
                Debug.Log(i + "번째에 랜덤 추가");
            }
        }
    }


    void SetAfterArray()
    {
        try
        {
            for (int i = 0; i < AfterNextStageArray.Length; i++)
            {
                if (MapList[1] == "랜덤")
                    AfterNextStageArray[i] = GetRandomStage();
                else
                    AfterNextStageArray[i] = MapList[1];
            }
        }
        catch // OutOfIndex가 떳을 때
        {
            Debug.Log(MapList.Count);
            if(MapList.Count <= 1)
            {
                for (int i = 0; i < AfterNextStageArray.Length; i++)
                    AfterNextStageArray[i] = null;
            }

            if(MapList.Count == 1)
            {
                int half = AfterNextStageArray.Length / 2;
                AfterNextStageArray[half] = "보스";
            }
        }
    }

    string GetRandomStage()
    {
        int randCount = 0;

        foreach (KeyValuePair<string, Stage> st in StageDict)
            randCount += st.Value.GetRemainCount();

        Debug.Log(randCount);

        // 가져올 수 있는 스테이지가 나올 때까지 무한히 돌리는 방법
        // 무한히 돌지 않도록 족쇄를 채워야 함(일단은 10번만 돌리는걸로)
        for(int i = 0; i < 10; i++)
        {
            int random = UnityEngine.Random.Range(0, randCount);

            foreach (KeyValuePair<string, Stage> st in StageDict)
            {
                random -= st.Value.GetRemainCount();

                if (0 < random)
                    continue;
               
                if (StageDict[st.Key].GetStage())
                    return st.Key;
                else
                    break;
            }
        }

        return null;
    }

    public void MoveNextStage(int index)
    {
        MapList.RemoveAt(0);

        for (int i = 0; i < NextStageArray.Length; i++)
        {
            if (NextStageArray[i] != null)
            {
                if (i != index)
                    StageDict[NextStageArray[i]].RecallCount();
            }

            NextStageArray[i] = AfterNextStageArray[index + i];
        }

        for(int i = 0; i < AfterNextStageArray.Length; i++)
        {
            if (AfterNextStageArray[i] == null)
                continue;

            if(i < index || index + 2 < i)
                StageDict[AfterNextStageArray[i]].RecallCount();
        }

        SetAfterArray();
    }


    // 디버그용 입력 이벤트
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            MoveNextStage(1);
    }
}
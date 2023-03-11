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

[Serializable]
public class Stage
{
    // 담고있는 정보가 어떤 스테이지의 것인지 확인하기 위한 변수
    public string Name;
    [SerializeField] string Type;     // 전투, 이벤트 등 
    [SerializeField] int MaxAppear;   // 필드에 출현 가능한 최대 갯수
    [SerializeField] int NowAppear;
    [SerializeField] int MaxCount;    // 최대 출현 가능 갯수
    [SerializeField] int RemainCount;

    public Stage(string type, int count, int appear)
    {
        Type = type;
        MaxAppear = appear;
        NowAppear = 0;
        MaxCount = RemainCount = count;
    }

    public string GetStageType() => Type;
    public int GetRemainCount() => RemainCount;

    public Stage Clone()
    {
        Stage cloneStage = new Stage(Type, MaxCount, MaxAppear);
        cloneStage.Name = Name;

        return cloneStage;
    }

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

    public void AppearClear() => NowAppear = 0;
}

public class StageManager : MonoBehaviour
{
    // 진행할 스테이지의 타입 리스트
    List<string> MapList = new List<string>();
    // 스테이지 정보
    List<Stage> StageInfo = new List<Stage>();
    
    // 디버그용 시리얼라이즈
    [SerializeField] Stage[] StageArray = new Stage[3];
    [SerializeField] Stage[] NextStageArray = new Stage[5];

    public Stage[] GetStageArray => StageArray;
    public Stage[] GetNextStageArray => NextStageArray;

    // 인스펙터에서 스테이지 정보를 받기 위해 만든 테스트용 리스트
    // StageManger를 생성하고 인스펙터에서 받아오는 식
    [SerializeField] public List<TestContainer> StageInfoContainer;
    
    // 현재 레벨에서 담고있는 스테이지의 정보
    [SerializeField] List<Stage> StageInfoList;
    // 맵의 막바지에 사용할 상점을 위한 리스트
    List<Stage> StoreList;


    private void Start()
    {
        GetStageInfo();
        InitStage();
    }
    
    void GetStageInfo()
    {
        StageInfo = new List<Stage>();
        StoreList = new List<Stage>();

        // 인스펙터에서 데이터를 받아서 StageInfoList에 넣는다.
        foreach (TestContainer test in StageInfoContainer)
        {
            Stage st = new Stage(test.Type, test.MaxCount, test.MaxAppear);
            st.Name = test.Name;

            if (st.GetStageType() == "Store")
                StoreList.Add(st);
            
            StageInfo.Add(st);
        }
    }


    void InitStage()
    {
        MapList.Clear();
        StageInfoList.Clear();

        SetStageData();
        SetMapList();

        // 다음 선택지를 모두 전투로 설정한다
        for(int i = 0; i < StageArray.Length; i++)
            StageArray[i] = FindStageByName(MapList[0]);
        SetNextArray();
    }

    // 인스펙터에서 받은 데이터를 입력하는 용도로 만듦
    // 데이터를 받는 방식이 달라지면 수정할 메서드
    void SetStageData()
    {
        foreach(Stage _stageInfo in StageInfo)
            StageInfoList.Add(_stageInfo.Clone());
    }

    void SetMapList()
    {
        for(int i =0; i < 4; i++)
        {
            if (i == 2)
                MapList.Add("Elite Battle");
            else
                MapList.Add("Common Battle");


            if (i <= 2)
                MapList.Add("Random");
            else
                MapList.Add("Store");


            if (i % 2 == 0)
                MapList.Add("Random");
        }
    }


    // AfterNextStageArray를 요구하는 타입에 맞게 스테이지를 배치
    void SetNextArray()
    {
        if (MapList.Count == 5)
        {
            Stage EliteStage = FindStageByName("Elite Battle");
            StageInfoList.Clear();
            SetStageData();

            Stage stage = FindStageByName("Elite Battle");
            stage = EliteStage;
        }
        
        try
        {
            NowAppearClear();

            for (int i = 0; i < NextStageArray.Length; i++)
            {
                if (MapList[1] == "Random")
                    NextStageArray[i] = GetRandomStage(i);
                else if(MapList[1] == "Store")
                {
                    int index = i;
                    if (StoreList.Count <= index)
                        index -= StoreList.Count;

                    NextStageArray[i] = StoreList[index];
                }
                else
                    NextStageArray[i] = FindStageByName(MapList[1]);
            }
        }
        catch // OutOfIndex가 떳을 때
        {
            Stage BossStage = new Stage("Battle", 0, 0);
            BossStage.Name = "Boss";

            for (int i = 0; i < NextStageArray.Length; i++)
                NextStageArray[i] = null;

            // MapList를 모두 소모했으면 Boss 출현
            if(MapList.Count == 1)
            {
                int half = NextStageArray.Length / 2;
                NextStageArray[half] = BossStage;
            }
            // Boss 스테이지를 가운데에 고정
            else if (MapList.Count == 0)
            {
                StageArray[0] = StageArray[2] = null;
                StageArray[1] = BossStage;
            }
        }
    }

    Stage GetRandomStage(int index)
    {
        int randCount = 0;

        foreach (Stage st in StageInfoList)
            randCount += st.GetRemainCount();

        // 가져올 수 있는 스테이지가 나올 때까지 무한히 돌리는 방법
        // 무한히 돌지 않도록 족쇄를 채워야 함(일단은 100번 돌리는걸로)
        for(int i = 0; i < 100; i++)
        {
            int random = UnityEngine.Random.Range(0, randCount);
            Debug.Log(i);
            foreach (Stage st in StageInfoList)
            {
                random -= st.GetRemainCount();

                if (0 < random)
                    continue;

                if (st.GetStageType() == "Store")
                {
                    bool CanStore = true;

                    for (int j = 0; j <= 2; j++)
                    {
                        int beforeStage = index - j;

                        if (0 <= beforeStage && beforeStage < StageArray.Length)
                        {
                            if(StageArray[beforeStage].Name == st.Name)
                            {
                                CanStore = false;
                                break;
                            }
                        }
                    }

                    if (!CanStore)
                        break;
                }
               
                if (st.GetStage())
                    return st;
                else
                    break;
            }
        }
        Debug.Log("return null");
        return null;
    }

    void NowAppearClear()
    {
        foreach(Stage st in StageInfo)
        {
            st.AppearClear();
        }
    }


    public void MoveNextStage(int index)
    {
        Debug.Log(StageArray[index].Name);
        Debug.Log(MapList[0]);

        for (int i = 0; i < StageArray.Length; i++)
        {
            if (StageArray[i] != null && MapList[0] == "Random")
            {
                if (i != index)
                    StageArray[i].RecallCount();
            }

            StageArray[i] = NextStageArray[index + i];
        }

        for(int i = 0; i < NextStageArray.Length; i++)
        {
            if (MapList[1] != "Random")
                break;

            if (NextStageArray[i] == null)
                continue;

            if(i < index || index + 2 < i)
                NextStageArray[i].RecallCount();
        }

        MapList.RemoveAt(0);
        SetNextArray();
    }

    Stage FindStageByName(string StageName) => StageInfoList.Find(x => x.Name == StageName);


    // 디버그용 입력 이벤트
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            MoveNextStage(1);
        if (Input.GetKeyDown(KeyCode.O))
            SceneChanger.SceneChange("StageSelectScene");
    }
}
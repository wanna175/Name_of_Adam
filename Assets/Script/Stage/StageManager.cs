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
    int MaxAppear;   // 필드에 출현 가능한 최대 갯수
    int NowAppear;
    int MaxCount;    // 최대 출현 가능 갯수
    int RemainCount;
    
    public Stage(string name, string type, int count, int appear)
    {
        Name = name;
        Type = type;
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

    // 현재 클래스의 내용물을 복사하여 새로운 객체를 반환
    public Stage Clone() => new Stage(Name, Type, MaxCount, MaxAppear);
    public void AppearClear() => NowAppear = 0;
}

public class StageManager : MonoBehaviour
{
    // 진행할 스테이지의 타입 리스트
    List<string> MapList = new List<string>();
    // 스테이지 정보의 컨테이너
    List<Stage> StageInfo = new List<Stage>();
    

    // 선택할 수 있는 다음 스테이지와 그 다음 표시되는 스테이지의 배열
    Stage[] StageArray = new Stage[3];
    Stage[] NextStageArray = new Stage[5];
    
    public Stage[] GetStageArray => StageArray;
    public Stage[] GetNextStageArray => NextStageArray;

    // 인스펙터에서 스테이지 정보를 받기 위해 만든 테스트용 리스트
    // StageManger를 생성하고 인스펙터에서 받아오는 식
    [SerializeField] public List<TestContainer> StageInfoContainer;
    
    // 현재 레벨에서 담고있는 스테이지의 정보
    List<Stage> StageInfoList;
    // 맵의 막바지에 사용할 상점을 위한 리스트
    List<Stage> StoreList;


    private void Start()
    {
        GetStageInfo();
        InitStage();
    }
    
    // 데이터 컨테이너에서 스테이지의 정보를 받아오는 메서드
    void GetStageInfo()
    {
        StageInfo = new List<Stage>();
        StoreList = new List<Stage>();

        // 인스펙터에서 데이터를 받아서 StageInfoList에 넣는다.
        foreach (TestContainer test in StageInfoContainer)
        {
            Stage st = new Stage(test.Name, test.Type, test.MaxCount, test.MaxAppear);

            if (st.GetStageType() == "Store")
                StoreList.Add(st);
            
            StageInfo.Add(st);
        }
    }

    // 스테이지의 최초 생성
    void InitStage()
    {
        MapList = new List<string>();
        StageInfoList = new List<Stage>();

        SetStageData();
        SetMapList();

        // 다음 선택지를 모두 전투로 설정한다.
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

    // 정해진 순서대로 MapList를 초기화
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


    // NextStageArray를 요구하는 타입에 맞게 스테이지를 배치
    void SetNextArray()
    {
        // 맵의 절반을 통과했으면, 엘리트 전투를 제외한 모든 스테이지의 횟수 초기화
        if (MapList.Count == 5)
        {
            Stage EliteStage = FindStageByName("Elite Battle");
            StageInfoList.Clear();
            SetStageData();

            Stage stage = FindStageByName("Elite Battle");
            stage = EliteStage;
        }

        if (MapList.Count > 1)
        {
            NowAppearClear();

            for (int i = 0; i < NextStageArray.Length; i++)
            {
                if (MapList[1] == "Random")
                    NextStageArray[i] = GetRandomStage(i);
                else if (MapList[1] == "Store")
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
        else // 맵의 끝에 도달했을 경우, 보스를 배치
        {
            Stage BossStage = new Stage("Boss", "Battle", 0, 0);

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
        // 등장할 수 있는 모든 스테이지 수의 합
        int randCount = 0;

        foreach (Stage st in StageInfoList)
            randCount += st.GetRemainCount();

        // 가져올 수 있는 스테이지가 나올 때까지 무한히 돌리는 방법
        // 무한히 돌지 않도록 족쇄를 채워야 함(일단은 100번 돌리는걸로)
        for(int i = 0; i < 100; i++)
        {
            int random = UnityEngine.Random.Range(0, randCount);
            
            foreach (Stage st in StageInfoList)
            {
                random -= st.GetRemainCount();

                if (0 < random)
                    continue;

                // 너무 빨리 엘리트 전투가 나오지 않도록 제한
                if (st.Name == "Elite Battle" && 7 < MapList.Count)
                    break;

                // 상점 뒤에 같은 상점이 나오지 않도록 제한
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
               
                // 제한사항이 없다면 스테이지 반환
                if (st.GetStage())
                    return st;
                else
                    break;
            }
        }
        
        return null;
    }

    void NowAppearClear()
    {
        foreach(Stage st in StageInfoList)
        {
            st.AppearClear();
        }
    }

    // 선택한 스테이지로 진행
    public void MoveNextStage(int index)
    {
        SetNextStage(StageArray[index]);


        if (MapList.Count <= 0)
        {
            // 모든 스테이지 소모
            // 다음 레벨로 진행

            return;
        }

        // 선택하지 않은 스테이지를 반환
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
            if (MapList.Count <= 1)
                break;

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

    // 다음 스테이지 이동용
    void SetNextStage(Stage stage)
    {
        Debug.Log("Now Stage : " + stage.Name);

        SceneChanger.SceneChange("JS TEST");
    }


    Stage FindStageByName(string StageName) => StageInfoList.Find(x => x.Name == StageName);


    // 디버그용 입력 이벤트
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            SceneChanger.SceneChange("StageSelectScene");
    }
}
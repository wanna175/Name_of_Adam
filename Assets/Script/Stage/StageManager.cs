using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    StageChanger _stageChanger;

    // 기존 시스템
    //// 진행할 스테이지의 타입 리스트
    List<MapSign> MapList;
    // 스테이지 정보의 컨테이너
    List<Stage> StageInfo;

    // 현재 레벨에서 담고있는 스테이지의 정보
    List<Stage> LocalStageInfo;


    // 선택할 수 있는 다음 스테이지와 그 다음 표시되는 스테이지의 배열
    Stage[] StageArray = new Stage[3];
    Stage[] NextStageArray = new Stage[3]; // 스마게용
    //Stage[] NextStageArray = new Stage[5];
    
    public Stage[] GetStageArray => StageArray;
    public Stage[] GetNextStageArray => NextStageArray;

    // 인스펙터에서 스테이지 정보를 받기 위해 만든 테스트용 리스트
    // StageManger를 생성하고 인스펙터에서 받아오는 식
    [SerializeField] public List<TestContainer> StageInfoContainer;


    private void Awake()
    {
        _stageChanger = new StageChanger();

        // 기존 시스템
        StageInfo = GameManager.Data.StageInfo;
        LocalStageInfo = GameManager.Data.LocalStageInfo;
        MapList = GameManager.Data.MapList;
        StageArray = GameManager.Data.StageArray;

        if (GameManager.Data.StageInfo.Count == 0)
        {
            GetStageInfo();
            InitStage();
        }

        GetStageInfo();
        InitStage();

        SetNextArray();
    }

    // 데이터 컨테이너에서 스테이지의 정보를 받아오는 메서드
    void GetStageInfo()
    {
        StageInfo = new List<Stage>();
         //인스펙터에서 데이터를 받아서 StageInfoList에 넣는다.
        foreach (TestContainer test in StageInfoContainer)
        {
            Stage st = new Stage(test.Name, test.Type, test.MaxCount, test.MaxAppear, test.Background);

            StageInfo.Add(st);
        }
    }

    // 스테이지의 최초 생성
    void InitStage()
    {
        /* 기존 시스템
        MapList = new List<string>();
        LocalStageInfo = new List<Stage>();

        SetStageData();
        SetMapList();
        // 다음 선택지를 모두 전투로 설정한다.
        for(int i = 0; i < StageArray.Length; i++)
            StageArray[i] = SetRandomFaction(MapList[0]);
        */
        SetRandomBattle(ref StageArray, GameManager.Data.SmagaStage[0].Stages); // 스마게용
        SetNextArray();
    }

    // 인스펙터에서 받은 데이터를 입력하는 용도로 만듦
    // 데이터를 받는 방식이 달라지면 수정할 메서드
    void SetStageData()
    {
        foreach (Stage _stageInfo in StageInfo)
        {
            Stage st = LocalStageInfo.Find(x => x.Name == _stageInfo.Name);

            if (st == null)
                LocalStageInfo.Add(_stageInfo.Clone());
            else
            {
                if (st.Name != StageName.EliteBattle)
                    st.InitCount();
            }
        }
    }

    // 정해진 순서대로 MapList를 초기화
    void SetMapList()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 2)
                MapList.Add(MapSign.EliteBattle);
            else
                MapList.Add(MapSign.CommonBattle);


            if (i <= 2)
                MapList.Add(MapSign.Random);
            else
                MapList.Add(MapSign.Store);


            if (i % 2 == 0)
                MapList.Add(MapSign.Random);
        }
    }


    // NextStageArray를 요구하는 타입에 맞게 스테이지를 배치
    void SetNextArray()
    {
        if (GameManager.Data.SmagaStage.Count > 1)
            SetRandomBattle(ref NextStageArray, GameManager.Data.SmagaStage[1].Stages); // 스마게용
        else
        {
            NextStageArray = new Stage[3];
            for (int i = 0; i < 3; i++)
                NextStageArray[i] = new Stage(StageName.none, StageType.Store, 0, 0, null);
        }

        /* 기존 시스템(스마게용에서 필요 x)
        // 맵의 절반을 통과했으면, 엘리트 전투를 제외한 모든 스테이지의 횟수 초기화
        if (MapList.Count == 5)
            SetStageData();

        if (MapList.Count > 1)
        {
            NowAppearClear();

            for (int i = 0; i < NextStageArray.Length; i++)
            {
                if (MapList[1] == MapSign.Store)
                {
                    int index = 0;
                    
                    for(int j = 0; j < NextStageArray.Length; j++)
                    {
                        if (StageInfo[index].GetStageType() == StageType.Store)
                            NextStageArray[j] = StageInfo[index];
                        else
                            j--;

                        index++;
                        if (index == StageInfo.Count)
                            index = 0;
                    }

                    break;
                }
                else if (MapList[1] == MapSign.Random)
                    NextStageArray[i] = GetRandomStage(i);
                else
                    NextStageArray[i] = SetRandomFaction(MapList[1]);
            }
        }
        else // 맵의 끝에 도달했을 경우, 보스를 배치
        {
            Stage BossStage = StageInfo.Find(x => x.Name == StageName.BossBattle);

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
        */
    }

    Stage GetRandomStage(int index)
    {
        // 이미 가져올 수 없음을 확인한 스테이지의 이름
        List<StageName> PassName = new List<StageName>();

        // 가져올 수 있는 스테이지가 나올 때까지 스테이지 확인
        while (true)
        {
            // 등장할 수 있는 모든 스테이지 수의 합
            int randCount = 0;

            foreach (Stage st in LocalStageInfo)
            {
                if (PassName.Contains(st.Name) == false)
                    randCount += st.GetRemainCount();
            }

            if (randCount == 0)
                break;

            int random = UnityEngine.Random.Range(0, randCount);


            foreach (Stage st in LocalStageInfo)
            {
                if (PassName.Contains(st.Name))
                    continue;

                random -= st.GetRemainCount();

                if (0 < random)
                    continue;


                // 너무 빨리 엘리트 전투가 나오지 않도록 제한
                if (st.Name == StageName.EliteBattle && 7 < MapList.Count)
                {
                    PassName.Add(st.Name);
                    break;
                }

                // 상점 뒤에 같은 상점이 나오지 않도록 제한
                if (st.GetStageType() == StageType.Store)
                {
                    bool CanStore = true;

                    for (int j = 0; j <= 2; j++)
                    {
                        int beforeStage = index - j;

                        if (0 <= beforeStage && beforeStage < StageArray.Length)
                        {
                            if (StageArray[beforeStage].Name == st.Name)
                            {
                                CanStore = false;
                                break;
                            }
                        }
                    }

                    if (!CanStore)
                    {
                        PassName.Add(st.Name);
                        break;
                    }
                }

                // 제한사항이 없다면 스테이지 반환
                if (!st.GetStage())
                {
                    PassName.Add(st.Name);
                    break;
                }

                return st;
            }
        }

        return null;
    }

    void NowAppearClear()
    {
        foreach (Stage st in LocalStageInfo)
        {
            st.AppearClear();
        }
    }

    // 선택한 스테이지로 진행
    public void MoveNextStage(int index)
    {
        // 스테이지 이동
        _stageChanger.SetNextStage(StageArray[index]);


        /* 기존 시스템, 스마게용에선 사용 안됨
        if (MapList.Count <= 0)
        {
            // 모든 스테이지 소모
            // 다음 레벨로 진행

            return;
        }

        // 선택하지 않은 스테이지를 반환
        for (int i = 0; i < StageArray.Length; i++)
        {
            if (StageArray[i] != null && MapList[0] == MapSign.Random)
            {
                if (i != index)
                    GetStageByName(StageArray[i].Name).RecallCount();
            }

            StageArray[i] = NextStageArray[index + i];
        }
        for(int i = 0; i < NextStageArray.Length; i++)
        {
            if (MapList.Count <= 1)
                break;

            if (MapList[1] != MapSign.Random)
                break;

            if (NextStageArray[i] == null)
                continue;

            if (i < index || index + 2 < i)
            {
                GetStageByName(NextStageArray[i].Name).RecallCount();
            }
        }
        */
        StageArray = (Stage[])NextStageArray.Clone(); // 스마게용
        GameManager.Data.SmagaStage.RemoveAt(0); // 스마게용

        //MapList.RemoveAt(0); 기존 시스템
        SetNextArray();
    }

    Stage GetStageByName(StageName name)
    {
        return LocalStageInfo.Find(x => x.Name == name);
    }

    Stage SetRandomFaction(MapSign sign)
    {
        StageName _name = (StageName)Enum.Parse(typeof(StageName), sign.ToString());
        Stage st = LocalStageInfo.Find(x => x.Name == _name);
        st.SetBattleFaction();

        return st.Clone();
    }

    // 스마게용 임시 매서드
    void SetRandomBattle(ref Stage[] inputArr, Stage[] stageData)
    {
        inputArr = stageData;
        for(int i = 0; i < 3; i++)
        {
            if (inputArr[i].GetStageType() == StageType.Battle)
            {
                int rand = UnityEngine.Random.Range(0, inputArr[i].BattleRandomStage.Length);

                inputArr[i].BattleStageData = inputArr[i].BattleRandomStage[rand];

                string name = inputArr[i].BattleStageData.faction.ToString();
                inputArr[i].Background = GameManager.Resource.Load<Sprite>("Arts/UI/Stage/" + name);
            }
        }
    }
}
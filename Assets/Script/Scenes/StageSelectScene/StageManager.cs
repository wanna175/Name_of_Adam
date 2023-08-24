using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class MapData
{
    public GameObject MapObject;      // 현재 맵 구조의 프리팹
    public List<StageData> StageList; // 맵의 노드들의 리스트
    public int CurrentTileID;         // 현재 위치하고있는 타일의 ID
    public List<int> ClearTileID;     // 이미 클리어한 타일의 ID

    public StageData GetStage(int ID) => StageList.Find(x => x.ID == ID);
    public StageData GetCurrentStage() => StageList.Find(x => x.ID == CurrentTileID);
}

public class StageManager : MonoBehaviour
{
    private static StageManager s_instance;
    public static StageManager Instance { get { Init(); return s_instance; } }

    [SerializeField] StageCameraController CameraController;
    StageChanger _stageChanger;

    List<Stage> StageList;
    Stage CurrentStage;


    private void Awake()
    {
        _stageChanger = new StageChanger();

        ActClearCheck();

        // 맵 프리팹이 존재하지 않다면(스테이지 최초 진입 시) 맵중에 랜덤으로 하나 가져오기
        if (GameManager.Data.Map.MapObject == null)
        {
            if (GameManager.Data.StageAct == 0)
                GameManager.Data.Map.MapObject = Resources.Load<GameObject>("Prefabs/Stage/TutorialMap");
            else
            {
                GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Stage/Maps");
                GameManager.Data.Map.MapObject = maps[UnityEngine.Random.Range(0, maps.Length)];
            }
        }
        Instantiate(GameManager.Data.Map.MapObject);
    }

    private void Start()
    {
        if (GameManager.Data.Map.StageList == null)
            SetStageData();
        SetCurrentStage();
        GameManager.VisualEffect.StartFadeEffect(true);
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@StageManager");
            s_instance = go.GetComponent<StageManager>();
        }
    }

    public void ActClearCheck()
    {
        // 마지막 스테이지일 때(ID가 99일 때)
        if (GameManager.Data.Map.CurrentTileID == 99)
        {
            if (GameManager.Data.StageAct < 1) // 여기의 상수는 최대 스테이지의 수
            {
                GameManager.Data.StageAct++;
                GameManager.Data.Map = new MapData();

                if(GameManager.Data.StageAct == 1)
                    GameManager.Data.MainDeckSet();
            }
            else
            {
                // 게임 종료
            }
        }
    }

    public void InputStageList(Stage stage)
    {
        if (StageList == null)
            StageList = new List<Stage>();
        if(!StageList.Contains(stage))
            StageList.Add(stage);
    }

    public void SetCurrentStage()
    {
        int curID = GameManager.Data.Map.CurrentTileID;
        CurrentStage = StageList.Find(x => x.Datas.ID == curID);

        foreach (Stage st in CurrentStage.NextStage)
            st.SetNextStage();

        CameraController.SetLocate(CurrentStage.transform.localPosition.y + 2);
    }

    private void SetStageData()
    {
        List<StageData> StageDatas = new List<StageData>();
        int maxLevel = 2; // 한 막에 몇가지 레벨이 존재하느냐에 따라 값이 증가함. ex) 한 맵에 3가지 레벨이 존재한다 -> maxLevel = 3
        int addLevel = (GameManager.Data.StageAct - 1) * maxLevel;
        List<Vector2> existStage = new List<Vector2>();

        for (int i = 0; i < StageList.Count; i++)
        {
            if (StageList[i].Datas.Type == StageType.Battle)
            {
                int x = (StageList[i].Datas.ID <= 1 && addLevel == 0) ? 0 : (int)StageList[i].Datas.StageLevel + addLevel;
                int y = UnityEngine.Random.Range(0, GameManager.Data.StageDatas[x].Count);

                Vector2 vec = new Vector2(x, y);

                if (!existStage.Contains(vec))
                {
                    existStage.Add(vec);
                    StageDatas.Add(StageList[i].SetBattleStage(x, y));
                }
                else
                    i--;
            }
            else
                StageDatas.Add(StageList[i].Datas);
        }

        GameManager.Data.Map.StageList = StageDatas;
    }

    public void StageMove(int _id)
    {
        foreach (Stage st in CurrentStage.NextStage)
        {
            if (st.Datas.ID == _id)
            {
                GameManager.VisualEffect.StartFadeEffect(false);
                GameManager.Instance.PlayAfterCoroutine(() => _stageChanger.SetNextStage(_id), 0.8f);
            }
        }
    }
}
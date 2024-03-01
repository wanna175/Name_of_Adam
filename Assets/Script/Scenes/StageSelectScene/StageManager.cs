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

    private List<Stage> StageList;
    public Stage CurrentStage;

    private void Awake()
    {
        _stageChanger = new StageChanger();

        ActClearCheck(); // 만약 클리어 체크가 될 시 Map이 초기화됨

        if (GameManager.Data.Map.MapObject == null)
            CreateMap();

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
        // 마지막 스테이지 조건을 보스일 때로 하고싶지만, 튜토리얼 스테이지의 경우에 의해 보스일 때로는 하지 못함
        if (GameManager.Data.Map.CurrentTileID == 99)
        {
            if (GameManager.Data.StageAct < 2) // 여기의 상수는 최대 막의 수, 지금은 1막밖에 없기에 1임
            {
                GameManager.Data.StageAct++;
                GameManager.Data.Map = new MapData();
            }
            else
            {
                // 게임 클리어
            }
        }
    }

    private void CreateMap()
    {
        if (GameManager.Data.StageAct == 0)
        {
            if (!GameManager.OutGameData.IsTutorialClear())
            {
                GameManager.Data.Map.MapObject = Resources.Load<GameObject>("Prefabs/Stage/Maps/TutorialMap/TutorialMap");
            }
            else
            {
                GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Stage/Maps/StageAct0");
                GameManager.Data.Map.MapObject = maps[UnityEngine.Random.Range(0, maps.Length)];
            }
        }
        else if (GameManager.Data.StageAct == 1)
        {
            GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Stage/Maps/StageAct1");
            GameManager.Data.Map.MapObject = maps[UnityEngine.Random.Range(0, maps.Length)];
        }
        else if (GameManager.Data.StageAct == 2)
        {
            GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Stage/Maps/StageAct2");
            GameManager.Data.Map.MapObject = maps[UnityEngine.Random.Range(0, maps.Length)];
        }
        else
        {
            GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Stage/Maps");
            GameManager.Data.Map.MapObject = maps[UnityEngine.Random.Range(0, maps.Length)];
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
        StartCoroutine(CurrentStage.Fade());

        foreach (Stage st in CurrentStage.NextStage)
        {
            if (st != null)
                st.SetNextStage();
        }


        CameraController.SetLocate(CurrentStage.transform.localPosition.y + 2);
    }

    private void SetStageData()
    {
        List<StageData> stageDatas = new();
        List<Vector2> existStage = new();

        for (int i = 0; i < StageList.Count; i++)
        {
            StageData stageData = StageList[i].Datas;

            int x = 0;
            int y = 0;

            if (stageData.Type == StageType.Battle && stageData.Name == StageName.CommonBattle) // 일반 배틀
            {
                if (GameManager.Data.StageAct == 0)
                {
                    if (stageData.ID < 4)
                    {
                        x = GameManager.OutGameData.IsHorusClear() ? 21 : (GameManager.OutGameData.IsPhanuelClear() ? 11 : 1);
                    }
                    else
                    {
                        x = GameManager.OutGameData.IsHorusClear() ? 22 : (GameManager.OutGameData.IsPhanuelClear() ? 12 : 2);
                    }
                }
                else if (GameManager.Data.StageAct == 1)
                {
                    x = GameManager.OutGameData.IsHorusClear() ? 23 : (GameManager.OutGameData.IsPhanuelClear() ? 13 : 3);
                }
                else if (GameManager.Data.StageAct == 2)
                {
                    x = GameManager.OutGameData.IsHorusClear() ? 24 : (GameManager.OutGameData.IsPhanuelClear() ? 14 : 4);
                }

                y = UnityEngine.Random.Range(0, GameManager.Data.StageDatas[x].Count);

                Vector2 vec = new Vector2(x, y);

                if (!existStage.Contains(vec))
                {
                    existStage.Add(vec);
                    stageDatas.Add(StageList[i].SetBattleStage(x, y));
                }
                else
                {
                    i--;
                }
            }
            else if (stageData.Type == StageType.Battle && stageData.Name == StageName.EliteBattle) // 엘리트 배틀
            {
                if (GameManager.OutGameData.IsHorusClear())
                {
                    x = stageData.StageLevel + 1;

                    if (GameManager.Data.StageAct == 0)
                    {
                        y = UnityEngine.Random.Range(0, 4);
                    }
                    else if(GameManager.Data.StageAct == 1)
                    {
                        y = UnityEngine.Random.Range(4, 11);
                    }
                }
                else if (GameManager.OutGameData.IsPhanuelClear())
                {
                    x = stageData.StageLevel;
                    y = GameManager.Data.StageAct == 0 ? 1 : 3;
                }
                else
                {
                    x = stageData.StageLevel;
                    y = GameManager.Data.StageAct == 0 ? 0 : 2;
                }

                if (!existStage.Contains(new Vector2(x, y)))
                {
                    existStage.Add(new Vector2(x, y));
                    stageDatas.Add(StageList[i].SetBattleStage(x, y));
                }
                else
                {
                    i--;
                }
            }
            else if (stageData.Type == StageType.Battle && stageData.Name == StageName.BossBattle) // 보스 배틀
            {
                if (GameManager.OutGameData.IsHorusClear())
                {
                    x = stageData.StageLevel + 1;
                    y = UnityEngine.Random.Range(0, GameManager.Data.StageDatas[x].Count);
                }
                else if (GameManager.OutGameData.IsPhanuelClear())
                {
                    x = stageData.StageLevel;
                    y = 1;
                }
                else
                {
                    x = stageData.StageLevel;
                    y = 0;
                }

                stageDatas.Add(StageList[i].SetBattleStage(x, y));
            }
            else
            {
                stageDatas.Add(stageData);
            }
        }

        GameManager.Data.Map.StageList = stageDatas;
    }

    public void StageMove(int id)
    {
        foreach (Stage st in CurrentStage.NextStage)
        {
            if (st != null && st.Datas.ID == id)
            {
                GameManager.Sound.Clear();
                GameManager.Sound.Play("Node/NodeClickSFX");
                GameManager.VisualEffect.StartFadeEffect(false);
                PlayAfterCoroutine(() => _stageChanger.SetNextStage(id), 0.8f);
            }
        }
    }

    public void PlayAfterCoroutine(Action action, float time) => StartCoroutine(PlayCoroutine(action, time));

    private IEnumerator PlayCoroutine(Action action, float time)
    {
        yield return new WaitForSeconds(time);

        action();
    }
}
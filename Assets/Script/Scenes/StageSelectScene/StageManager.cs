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
    private StageChanger _stageChanger;

    private List<Stage> StageList;
    public Stage CurrentStage;

    private bool _isClicked = false;

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
        if (GameManager.Data.Map.MapObject == null)
        {
            string mapFolderPath = "Prefabs/Stage/Maps/";

            if (GameManager.Data.StageAct == 0 && !GameManager.OutGameData.IsTutorialClear())
            {
                mapFolderPath += "TutorialMap";
            }
            else
            {
                mapFolderPath += "StageAct" + GameManager.Data.StageAct;
            }

            GameObject[] maps = Resources.LoadAll<GameObject>(mapFolderPath);

            GameManager.Data.Map.MapObject = maps[UnityEngine.Random.Range(0, maps.Length)];
        }
    }

    public void InputStageList(Stage stage)
    {
        if (StageList == null)
            StageList = new();
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
         _isClicked = false;

        CameraController.SetLocate(CurrentStage.transform.localPosition.y + 2);
    }

    private void SetStageData()
    {
        List<StageData> stageDataList = new();
        List<Vector2> existStage = new();

        for (int i = 0; i < StageList.Count; i++)
        {
            StageData stageData = StageList[i].Datas;

            if (stageData.Type == StageType.Battle)
            {
                int stageLevel = 0;
                int stageID = 0;

                if (stageData.Name == StageName.CommonBattle) // 일반 배틀
                {
                    if (GameManager.Data.StageAct == 0)
                    {
                        stageLevel = stageData.ID < 4 ? 1 : 2;
                    }
                    else if (GameManager.Data.StageAct == 1)
                    {
                        stageLevel = 3;
                    }
                    else if (GameManager.Data.StageAct == 2)
                    {
                        stageLevel = 4;
                    }

                    if (GameManager.OutGameData.IsHorusClear())
                    {
                        stageLevel += 20;
                    }
                    else if (GameManager.OutGameData.IsPhanuelClear())
                    {
                        stageLevel += 10;
                    }

                    stageID = UnityEngine.Random.Range(0, GameManager.Data.StageDatas[stageLevel].Count);
                }
                else if (stageData.Name == StageName.EliteBattle) // 엘리트 배틀
                {
                    stageLevel = stageData.StageLevel;

                    if (GameManager.OutGameData.IsHorusClear())
                    {
                        stageLevel += 1;

                        if (GameManager.Data.StageAct == 0)
                        {
                            stageID = UnityEngine.Random.Range(0, 4);
                        }
                        else if (GameManager.Data.StageAct == 1)
                        {
                            stageID = UnityEngine.Random.Range(4, 11);
                        }
                    }
                    else if (GameManager.OutGameData.IsPhanuelClear())
                    {
                        stageID = GameManager.Data.StageAct == 0 ? 1 : 3;
                    }
                    else
                    {
                        stageID = GameManager.Data.StageAct == 0 ? 0 : 2;
                    }
                }
                else if (stageData.Name == StageName.BossBattle) // 보스 배틀
                {
                    stageLevel = stageData.StageLevel;

                    if (GameManager.OutGameData.IsHorusClear())
                    {
                        stageLevel += 1;
                        stageID = UnityEngine.Random.Range(0, GameManager.Data.StageDatas[stageLevel].Count);
                    }
                    else if (GameManager.OutGameData.IsPhanuelClear())
                    {
                        stageID = 1;
                    }
                    else
                    {
                        stageID = 0;
                    }
                }

                Vector2 vec = new(stageLevel, stageID);

                if (!existStage.Contains(vec))
                {
                    existStage.Add(vec);
                    stageDataList.Add(StageList[i].SetBattleStage(stageLevel, stageID));
                }
                else
                {
                    i--;
                }
            }
            else
            {
                stageDataList.Add(stageData);
            }
        }

        GameManager.Data.Map.StageList = stageDataList;
    }

    public void StageMove(int id)
    {
        if (_isClicked)
            return;

        foreach (Stage st in CurrentStage.NextStage)
        {
            if (st != null && st.Datas.ID == id)
            {
                _isClicked = true;

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
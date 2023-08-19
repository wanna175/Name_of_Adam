using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class MapData
{
    public List<StageData> StageList;
    public int CurrentTileID;
    public List<int> ClearTileID;
}

public class StageManager : MonoBehaviour
{
    private static StageManager s_instance;
    public static StageManager Instance { get { Init(); return s_instance; } }

    List<Stage> StageList;
    [SerializeField] Stage CurrentStage;
    [SerializeField] StageCameraController CameraController;

    StageChanger _stageChanger;


    private void Awake()
    {
        _stageChanger = new StageChanger();
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

    public void InputStageList(Stage stage)
    {
        if (StageList == null)
            StageList = new List<Stage>();
        StageList.Add(stage);
    }

    public void SetCurrentStage()
    {
        int curID = GameManager.Data.Map.CurrentTileID;

        CurrentStage = StageList.Find(x => x.Datas.ID == curID);
        if(curID == 17)
        {
            CameraController.SetLocate(-5);
        }
        else
        {
            CameraController.SetLocate(CurrentStage.transform.localPosition.y + 2);
        }
            
    }

    private void SetStageData()
    {
        List<StageData> StageDatas = new List<StageData>();
        int addLevel = GameManager.Data.StageAct * 2;

        foreach (Stage value in StageList)
        {
            if (value.Datas.Type == StageType.Battle)
            {
                int x = (value.Datas.ID <= 1 && addLevel == 0) ? 0 : (int)value.Datas.StageLevel + addLevel;
                int y = UnityEngine.Random.Range(0, GameManager.Data.StageDatas[x].Count);

                if (value.Datas.ID <= 3)
                {
                    y = 0;
                }

                StageDatas.Add(value.SetBattleStage(x, y));
            }
            else
                StageDatas.Add(value.Datas);
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
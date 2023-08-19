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

        foreach (Stage st in CurrentStage.NextStage)
            st.SetNextStage();

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

        foreach (StageData data in GameManager.Data.Map.StageList)
        {
            Debug.Log(data.ID + ", " + data.Name + " : " + data.StageID + ", " + data.StageLevel);
        }
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
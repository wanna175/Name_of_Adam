using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StageType
{
    Store,
    Event,
    Battle,
    Tutorial
}

[Serializable]
public class StageData
{
    public int ID;
    public StageType Type;
    public StageName Name;
    public int StageLevel;
    public int StageID;
}

public class Stage : MonoBehaviour
{
    [SerializeField] StageNodeBackLight BackLight;
    [Space(10f)]
    [SerializeField] public List<Stage> NextStage;
    [Space(10f)]
    [Header("StageInfo")]
    [SerializeField] public StageData Datas;
    private bool isClear = false;
    private bool isNextStage = false;


    private void Awake()
    {
        StageManager.Instance.InputStageList(this);
    }
    private void Start()
    {
        if (GameManager.Data.Map.ClearTileID == null)
            GameManager.Data.Map.ClearTileID = new();

        if (GameManager.Data.Map.ClearTileID.Contains(Datas.ID))
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            isClear = true;
        }

        foreach (Stage st in NextStage)
        {
            StageLine line = GameManager.Resource.Instantiate("Stage/Line", transform).GetComponent<StageLine>();
            line.DrawLine(st);
        }
    }

    public void SetNextStage()
    {
        isNextStage = true;
        BackLight.SetVisible();
    }

    public void OnMouseUp() => StageManager.Instance.StageMove(Datas.ID);

    public void OnMouseEnter()
    {
        if (isNextStage)
            return;
        if (!isClear)
            BackLight.FadeIn();
    }

    public void OnMouseExit()
    {
        if (isNextStage)
            return;
        if (!isClear)
            BackLight.FadeOut();
    }

    public StageData SetBattleStage(int a, int b)
    {
        Datas.StageLevel = a;
        Datas.StageID = b;

        return Datas;
    }
}
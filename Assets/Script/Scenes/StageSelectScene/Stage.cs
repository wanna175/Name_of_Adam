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
    SpriteRenderer renderer;

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
        renderer = GetComponent<SpriteRenderer>();
        StageManager.Instance.InputStageList(this);
    }
    private void Start()
    {
        SetSprite();

        if (GameManager.Data.Map.ClearTileID == null)
            GameManager.Data.Map.ClearTileID = new();

        if (GameManager.Data.Map.ClearTileID.Contains(Datas.ID))
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            isClear = true;
        }

        foreach (Stage st in NextStage)
        {
            if (Datas.ID == 0)
                return;

            StageLine line = GameManager.Resource.Instantiate("Stage/Line", transform).GetComponent<StageLine>();
            line.DrawLine(st);
        }
    }

    public void SetSprite()
    {
        if (Datas.Name == StageName.none)
            renderer.color = new Color(1, 1, 1, 0);

        string name = Datas.Name.ToString();

        if (Datas.Name == StageName.EliteBattle || Datas.Name == StageName.BossBattle)
            name += "_" + Datas.StageID;

        renderer.sprite = GameManager.Resource.Load<Sprite>($"Arts/StageSelect/Node/{name}");
        BackLight.SetSprite(name);
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
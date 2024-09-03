using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
    public int ID; //각 노드가 가지는 고유식별번호
    public StageType Type; //노드가 가진 스테이지의 종류
    public StageName Name; //Type에서 더 세분화된 스테이지의 내용
    public int StageLevel; //전투 스테이지에서 사용하는 현재 전투의 레벨
    public int StageID; //전투 스테이지에서 사용하는 현재 전투의 ID, 99면 보스
}

public class Stage : MonoBehaviour
{
    private SpriteRenderer _renderer;

    [SerializeField] public StageNodeBackLight BackLight;
    [Space(10f)]
    [SerializeField] public List<Stage> NextStage;
    [Space(10f)]
    [Header("StageInfo")]
    [SerializeField] public StageData Datas;

    public void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();

        SetSprite();

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
        {
            _renderer.color = new Color(1, 1, 1, 0);
            return;
        }

        string name = Datas.Name.ToString();

        if (Datas.Name == StageName.EliteBattle || Datas.Name == StageName.BossBattle)
        {
            name += "_" + GameManager.Data.StageDatas[Datas.StageLevel][Datas.StageID].Units[0].Name;
        }

        _renderer.sprite = GameManager.Resource.Load<Sprite>($"Arts/StageSelect/Node/{name}");
        BackLight.SetSprite(name);

        if (GameManager.Data.Map.ClearTileID.Contains(Datas.ID))
        {
            _renderer.color = new Color(0, 0, 0);
        }
    }

    public IEnumerator Fade()
    {
        float FadeTime = 1;
        float time = 0;

        while(time < FadeTime)
        {
            time += Time.deltaTime;
            float t = time / FadeTime;

            _renderer.color = new Color(1 - t, 1 - t, 1 - t);

            yield return null;
        }
    }

    public StageData SetBattleStage(int stageLevel, int stageID)
    {
        Datas.StageLevel = stageLevel;
        Datas.StageID = stageID;

        return Datas;
    }

    public void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            StageManager.Instance.StageMouseClick(this);
        }
    }

    public void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            StageManager.Instance.StageMouseEnter(this);
        }
    }

    public void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            StageManager.Instance.StageMouseExit(this);
        }
    }
}
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
    public int ID; //�� ��尡 ������ �����ĺ���ȣ
    public StageType Type; //��尡 ���� ���������� ����
    public StageName Name; //Type���� �� ����ȭ�� ���������� ����
    public int StageLevel; //���� ������������ ����ϴ� ���� ������ ����
    public int StageID; //���� ������������ ����ϴ� ���� ������ ID, 99�� ����
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
            this.transform.localScale = (Datas.Name == StageName.EliteBattle) ? new(2f, 2f, 2f) : new(2.5f, 2.5f, 2.5f);
            GetComponent<BoxCollider2D>().size = (Datas.Name == StageName.EliteBattle) ? new(3, 3) : new(5, 5);
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
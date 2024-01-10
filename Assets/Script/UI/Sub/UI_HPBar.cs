using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    [Header("HP 관련")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _playerBar;
    [SerializeField] private Image _enemyBar;

    [Header("타락 게이지 관련")]
    [SerializeField] private Transform _grid;
    [SerializeField] private GameObject _fallGaugeUnit; // 타락 게이지 각 보석

    [Header("버프 관련")]
    [SerializeField] private Transform _buffGrid;
    [SerializeField] private GameObject _buff; // 타락 게이지 각 보석

    private List<UI_FallUnit> _fallGauge = new();
    private int _fallCountIdx = -1;
    private int _UnitfallGaugeMax = 0;
    private int _UnitfallGaugeCur = 0;
    private List<UI_Buff> _buffBlockList = new();
    private int _rotationMax = 0;
    private int _rotationCurrent = 0;

    private Team _team;

    private void Start()
    {
        Rotation();
    }

    public void SetHPBar(Team team)
    {
        if (team == Team.Player)
        {
            _playerBar.gameObject.SetActive(true);
            _enemyBar.gameObject.SetActive(false);
        }
        else
        {
            _enemyBar.gameObject.SetActive(true);
            _playerBar.gameObject.SetActive(false);
        }

        _team = team;
    }

    public void RefreshHPBar(float amount)
    {
        if (_team == Team.Player)
            _playerBar.fillAmount = amount;
        else
            _enemyBar.fillAmount = amount;
    }

    public void SetFallBar(DeckUnit unit) // unit FallMaxCount에 따라서 일단 fallbar를 셋팅한다.
    {
        int max = unit.DeckUnitTotalStat.FallMaxCount;
        int current = unit.DeckUnitTotalStat.FallCurrentCount;
        if (max > 4 && _team == Team.Player)
        {
            max = 4;
        }
       
        _UnitfallGaugeMax = max;
        _UnitfallGaugeCur = max;//마름모의 갯수
        if (_fallGauge.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                UI_FallUnit newObject = GameObject.Instantiate(_fallGaugeUnit, _grid).GetComponent<UI_FallUnit>();
                newObject.SwitchCountImage(_team);
                _fallGauge.Add(newObject);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            _fallGauge[i].SwitchCountImage(_team);
            _fallGauge[i].EmptyGauge();
        }
        
        if (_UnitfallGaugeCur > 4)
        {
            int doubleCnt = _UnitfallGaugeCur - 4;
            _fallCountIdx = doubleCnt - 1; 
            for(int i = 0; i < doubleCnt; ++i)
            {
                _fallGauge[i].EmptyGauge();
            }
        }
        else
        {
            _fallCountIdx = _UnitfallGaugeCur-1;
            for (int i = 3; i > _fallCountIdx; --i)
            {
                _fallGauge[i].FillGauge();
            }
        }
        
    }

    public void RefreshFallGauge(int current)
    {

        int diff = _UnitfallGaugeCur - (_UnitfallGaugeMax - current);
        _UnitfallGaugeCur = _UnitfallGaugeMax - current;
        if (diff == 1)
        {
            _fallGauge[_fallCountIdx--].FillGauge();
            if (_fallCountIdx < 0 && _UnitfallGaugeMax != _UnitfallGaugeCur)
                _fallCountIdx = 3;
        }
        else if (diff == -1)
        {
            _fallCountIdx++;
            if (_fallCountIdx == 4 && _fallGauge[3].GetDouble()!=2)
                _fallCountIdx = 0;
            _fallGauge[_fallCountIdx].EmptyGauge();
        }
        else
            Debug.Log("Same");

        for (int i = 0; i < 4; ++i)
        {
            _fallGauge[i].SwitchCountImage(_team);
        }
    }

    public void AddBuff(Buff buff)
    {
        if (buff.StigmaBuff || buff.Sprite == null)
            return;

        foreach (UI_Buff listedBuff in _buffBlockList)
        {
            if (buff.BuffEnum == listedBuff.BuffInBlock.BuffEnum)
            {
                return;
            }
        }

        UI_Buff newBuff = GameObject.Instantiate(_buff, _buffGrid).GetComponent<UI_Buff>();
        newBuff.Set(this, buff);
        newBuff.gameObject.SetActive(false);

        _buffBlockList.Add(newBuff);
    }

    public void DeleteBuff(BuffEnum buffEnum)
    {
        for (int i = 0; i < _buffBlockList.Count; i++)
        {
            if (_buffBlockList[i].BuffInBlock.BuffEnum == buffEnum)
            {
                UI_Buff buff = _buffBlockList[i];
                _buffBlockList.RemoveAt(i);
                Destroy(buff.gameObject);
                break;
            }
        }
    }

    private bool rotateStop = false;

    public void BuffHoverIn()
    {
        rotateStop = true;
    }

    public void BuffHoverOut()
    {
        rotateStop = false;
    }

    public void Rotation()
    {
        Invoke(nameof(Rotation), 2f);
        if (rotateStop)
            return;

        foreach (UI_Buff listedBuff in _buffBlockList)
        {
            listedBuff.gameObject.SetActive(false);
        }

        int count = 0;

        for (int i = 0 + _rotationCurrent * 3; i < _buffBlockList.Count; i++)
        {
            if (count == 3)
                break;

            _buffBlockList[i].gameObject.SetActive(true);

            count++;
        }

        _rotationCurrent++;

        if (_rotationCurrent > _rotationMax)
        {
            _rotationCurrent = 0;
        }
    }

    public void RefreshBuff()
    {
        _rotationMax = (_buffBlockList.Count-1) / 4;

        foreach (UI_Buff listedBuff in _buffBlockList)
        {
            listedBuff.RefreshBuffDisplayNumber();
        }
    }
}

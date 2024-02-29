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
    private int _rotationCurrent = 0;

    private Team _team;

    private void Start()
    {
        Rotation();
    }

    public void SetPosition(BattleUnit unit, bool resetPosition = false)
    {
        if (unit.DeckUnit.GetUnitSize() > 1 && !resetPosition)
        {
            float position = unit.transform.position.x;
            foreach (ConnectedUnit connectedUnit in unit.ConnectedUnits)
            {
                position += connectedUnit.transform.position.x;
            }

            this.transform.position = new Vector3(position / (unit.ConnectedUnits.Count + 1), this.transform.position.y, this.transform.position.z);
        }
        else
        {
            this.transform.localPosition = new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z);
        }
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
        _UnitfallGaugeCur = max-current;//마름모의 갯수
        if (_fallGauge.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                UI_FallUnit newObject = GameObject.Instantiate(_fallGaugeUnit, _grid).GetComponent<UI_FallUnit>();
                newObject.SwitchCountImage(_team);
                newObject.gameObject.SetActive(false);
                _fallGauge.Add(newObject);
            }
        }

        for (int i = 0; i < _UnitfallGaugeCur; i++)
        {
            int idx = i;
            if (i > 3) { 
                idx -= 4;}
            if (i > 7)
                idx -= 4;
            _fallGauge[idx].SwitchCountImage(_team);
            _fallGauge[idx].EmptyGauge();
        }
        
        if (_UnitfallGaugeCur > 4&&_UnitfallGaugeCur<=8)
        {
            int doubleCnt = _UnitfallGaugeCur - 4;
            _fallCountIdx = doubleCnt - 1; 
        }
        else if (_UnitfallGaugeCur > 8)
        {
            _fallCountIdx = _UnitfallGaugeCur - 9;
        }
        else
        {
            _fallCountIdx = _UnitfallGaugeCur-1;
        }
        
    }

    public void RefreshFallGauge(int current)
    {
        int diff = _UnitfallGaugeCur - (_UnitfallGaugeMax - current);
        _UnitfallGaugeCur = _UnitfallGaugeMax - current;
        if (diff == 1)
        {
            Debug.Log("idx qqqqqqqqqqqqq: " + _fallCountIdx);
            _fallGauge[_fallCountIdx--].FillGauge();
            if (_fallCountIdx < 0 && _UnitfallGaugeMax != _UnitfallGaugeCur)
                _fallCountIdx = 3;
        }
        else if (diff == -1)
        {
            _fallCountIdx++;
            if (_fallCountIdx == 4 && _fallGauge[3].GetDouble() != 2)
                _fallCountIdx = 0;
            else if (_fallCountIdx == 4 && _fallGauge[3].GetDouble() != 3)
                _fallCountIdx = 0;
            if (_fallCountIdx == 4 && _fallGauge[3].GetDouble() == 3)
                Debug.Log("최대갯수를 초과 하였습니다.");
            else
                _fallGauge[_fallCountIdx].EmptyGauge();
        }

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

        if (_rotationCurrent > _buffBlockList.Count / 4)
        {
            _rotationCurrent = 0;
        }
    }

    public void RefreshBuff()
    {
        foreach (UI_Buff listedBuff in _buffBlockList)
        {
            listedBuff.RefreshBuffDisplayNumber();
        }
    }
}

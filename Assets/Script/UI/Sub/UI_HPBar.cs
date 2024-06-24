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
    [SerializeField] private Transform[] _fallGrids;
    [SerializeField] private GameObject _fallGaugeUnit; // 타락 게이지 각 보석 

    [Header("버프 관련")]
    [SerializeField] private Transform _buffGrid;
    [SerializeField] private GameObject _buff; // 타락 게이지 각 보석

    private List<UI_FallUnit> _fallGauge = new();
    private int _fallCount;
    private int _currentIndex; // 4개의 보석 중 Change가 발생할 될 보석 위치

    private List<UI_Buff> _buffBlockList = new();
    public List<UI_Buff> BuffBlockList => _buffBlockList;
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
        int maxFallNum = unit.DeckUnitTotalStat.FallMaxCount;
        int curFallNum = unit.DeckUnitTotalStat.FallCurrentCount;

        if (maxFallNum > 4 && _team == Team.Player)
        {
            maxFallNum = 4;
        }

        // 모든 신앙 보석 제거
        foreach (UI_FallUnit listedFall in _fallGauge)
            Destroy(listedFall.gameObject);
        _fallGauge.Clear();
        _fallCount = curFallNum;
        _currentIndex = maxFallNum - curFallNum - 1;

        // 보석 생성 및 초기화
        for (int i = 0; i < maxFallNum; i++)
        {
            int fallType = i / 4;
            UI_FallUnit newObject = GameObject.Instantiate(_fallGaugeUnit, _fallGrids[fallType]).GetComponent<UI_FallUnit>();

            newObject.InitFall(_team, fallType);
            if (i >= maxFallNum - curFallNum)
                newObject.SetVisible(false);

            _fallGauge.Add(newObject);
        }
    }

    public void RefreshFallBar(int current, FallAnimMode fallAnimMode = FallAnimMode.On, float fallAnimDelay = 0.75f)
    {
        int gap = current - _fallCount; // 신앙 보석 차이
        int count = Mathf.Abs(gap);

        for (int i = 0; i < count; i++)
        {
            if (gap > 0)
            {
                // 신앙 감소 = 보석 파괴 (역순)
                switch (fallAnimMode)
                {
                    case FallAnimMode.On:
                        _fallGauge[_currentIndex].SetVisible(true);
                        _fallGauge[_currentIndex].DecreaseGauge(fallAnimDelay); 
                        break;

                    case FallAnimMode.Off: 
                        _fallGauge[_currentIndex].SetVisible(false);
                        break;
                }
                _fallCount++;
                _currentIndex--;
            }
            else
            {
                // 신앙 증가 = 보석 복구 (순)
                _fallCount--;
                _currentIndex++;
                switch (fallAnimMode)
                {
                    case FallAnimMode.On:
                        _fallGauge[_currentIndex].SetVisible(true);
                        _fallGauge[_currentIndex].IncreaseGauge(fallAnimDelay);
                        break;

                    case FallAnimMode.Off:
                        _fallGauge[_currentIndex].SetVisible(false);
                        break;
                }
            }
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
        if (_buffBlockList.Count > 3)
        {
            newBuff.gameObject.SetActive(false);
        }

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
                if (_isBuffHoverIn)
                {
                    GameManager.UI.CloseHover();
                }
                break;
            }
        }
    }

    private bool _isBuffHoverIn = false;

    public void BuffHoverIn()
    {
        _isBuffHoverIn = true;
    }

    public void BuffHoverOut()
    {
        _isBuffHoverIn = false;
    }

    public void Rotation()
    {
        Invoke(nameof(Rotation), 2f);
        if (_isBuffHoverIn)
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

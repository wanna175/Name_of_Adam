using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDataManager
{
    private List<Unit> _UnitList = new List<Unit>();
    public List<Unit> UnitList => _UnitList;

    public void AddUnit(Unit unit) {
        UnitList.Add(unit);
    }

    public void RemoveUnit(Unit unit) {
        UnitList.Remove(unit);
    }

    public Unit GetRandomUnit() {
        if (UnitList.Count == 0)
        {
            return null;
        }
        int randNum = Random.Range(0, UnitList.Count);
        
        Unit unit = UnitList[randNum];
        _UnitList.RemoveAt(randNum);

        return unit;
    }
    #region BattleUnitList
    
    // 전투를 진행중인 캐릭터가 들어있는 리스트
    #region BattleUnitList  
    List<BattleUnit> _BattleUnitList = new List<BattleUnit>();
    public List<BattleUnit> BattleUnitList => _BattleUnitList;
    #endregion

    // 현재 리스트를 초기화
    public void UnitListClear() => BattleUnitList.Clear();

    // 리스트에 캐릭터를 추가 / 제거
    public void BattleUnitEnter(BattleUnit unit) => BattleUnitList.Add(unit);

    public void BattleUnitExit(BattleUnit unit) => BattleUnitList.Remove(unit);
    #endregion

    #region Mana

    #region ManaCost
    private const int _MaxManaCost = 10;

    private int _ManaCost = 0;
    public int ManaCost => _ManaCost;
    #endregion

    UI_ManaGuage _manaGuage;

    public void InitMana(int _defaultMana = 0) => _ManaCost = _defaultMana;

    public void SetManaGuage(UI_ManaGuage _guage)
    {
        _manaGuage = _guage;
    }

    public void ChangeMana(int value)
    {
        _ManaCost += value;

        if (_MaxManaCost <= _ManaCost)
            _ManaCost = _MaxManaCost;
        else if (_ManaCost < 0)
            _ManaCost = 0;
        
        _manaGuage.DrawGauge();
    }

    public bool CanUseMana(int value)
    {
        if (_ManaCost >= value)
            return true;
        else
        {
            //마나 부족
            Debug.Log("not enough mana");
            return false;
        }
    }
    #endregion
}
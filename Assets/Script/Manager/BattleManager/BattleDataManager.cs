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
    
    // 전투를 진행중인 캐릭터가 들어있는 리스트
    List<BattleUnit> _battleUnitList = new List<BattleUnit>();
    public List<BattleUnit> BattleUnitList => _battleUnitList;

    // 현재 리스트를 초기화
    public void UnitListClear() => BattleUnitList.Clear();

    // 리스트에 캐릭터를 추가 / 제거
    public void BattleUnitAdd(BattleUnit unit) => BattleUnitList.Add(unit);

    public void BattleUnitRemove(BattleUnit unit) => BattleUnitList.Remove(unit);



    /*================================마나 데이터 관련=================================*/

    private const int _maxMana = 100;

    private int _mana = 0;
    
    public void InitMana(int _defaultMana = 0) => _mana = _defaultMana;

    public int GetMana()
    {
        return _mana;
    }

    public void ChangeMana(int value)
    {
        _mana += value;

        if (_maxMana <= _mana)
            _mana = _maxMana;
        else if (_mana < 0)
            _mana = 0;
    }

    public bool CanUseMana(int value)
    {
        if (_mana >= value)
            return true;

        Debug.Log("not enough mana");
        return false;
    }
}
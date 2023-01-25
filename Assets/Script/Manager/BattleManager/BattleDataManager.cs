using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDataManager
{
    #region DataMNG
    private DataManager _DataMNG;
    public DataManager DataMNG => _DataMNG;
    #endregion

    // 필드와 배틀유닛, 마나의 정보 및 관리는 각 매니저로 분할

    public BattleDataManager()
    {
        _DataMNG = GameManager.Instance.DataMNG;
    }
     
    #region Turn
    private int _Turn;
    public int Turn => _Turn;

    public void TurnPlus()
    {
        _Turn++;
    }
    #endregion

    #region Prepare / Engage Stage
    private bool _EngageStage;
    public bool EngageStage => _EngageStage;
    //즉 현재 상태가 전투 단계면 참이니
    //거짓을 반환시 준비 단계이다.
    public void SetEngageStage(bool stage)
    {
        _EngageStage = stage;
    }
    #endregion

    #region DeckUnitList
    private List<DeckUnit> _DeckUnitList = new List<DeckUnit>();
    public List<DeckUnit> DeckUnitList => _DeckUnitList;

    public void CopyDeckList(DeckUnit unit) {
        foreach (DeckUnit d in DataMNG.DeckUnitList)
        {
            AddDeckUnit(d);
        }
    }

    public void AddDeckUnit(DeckUnit unit) {
        DeckUnitList.Add(unit);
    }

    public void RemoveDeckUnit(DeckUnit unit) {
        DeckUnitList.Remove(unit);
    }

    public DeckUnit GetRandomDeckUnit() {
        if (DeckUnitList.Count == 0)
        {
            return null;
        }
        int randNum = Random.Range(0, DeckUnitList.Count);
        
        DeckUnit unit = DeckUnitList[randNum];
        _DeckUnitList.RemoveAt(randNum);

        return unit;
    }

    #endregion

    #region BattleUnitList
    
    // 전투를 진행중인 캐릭터가 들어있는 리스트
    #region BattleUnitList  
    List<BattleUnit> _BattleUnitList = new List<BattleUnit>();
    public List<BattleUnit> BattleUnitList => _BattleUnitList;
    #endregion  

    // 리스트에 캐릭터를 추가 / 제거
    #region UnitEnter / Exit
    public void BattleUnitEnter(BattleUnit unit)
    {
        BattleUnitList.Add(unit);
    }
    public void BattleUnitExit(BattleUnit unit)
    {
        BattleUnitList.Remove(unit);
    }
    #endregion

    // 현재 리스트를 초기화
    public void UnitListClear()
    {
        BattleUnitList.Clear();
    }

    //필드에 유닛을 생성
    public void CreatBattleUnit(GameObject BattleUnitPrefab, int x, int y)
    {
        BattleUnit BattleUnit = BattleUnitPrefab.GetComponent<BattleUnit>();

        BattleUnit.BattleUnitSO = GameManager.Instance.UIMNG.Hands.ClickedUnit.GetUnitSO();
        BattleUnit.UnitMove.setLocate(x, y);

        GameManager.Instance.FieldMNG.EnterTile(BattleUnit, x, y);
    }

    #endregion

    #region Mana

    #region ManaCost
    private int _ManaCost = 0;
    public int ManaCost => _ManaCost;
    #endregion

    UI_ManaGuage _manaGuage;

        public void InitMana()
    {
        _ManaCost = 0;
    }

    public void SetManaGuage(UI_ManaGuage _guage)
    {
        _manaGuage = _guage;
    }

    public void ChangeMana(int value)
    {
        _ManaCost += value;

        if (10 <= _ManaCost)
            _ManaCost = 10;
        else if (_ManaCost < 0)
            _ManaCost = 0;
        
        _manaGuage.DrawGauge();
    }

    public bool CanUseMana(int value)
    {
        if (_ManaCost >= value)
            return true;
        else
            return false;
    }
    #endregion
}

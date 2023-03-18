using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDataManager : MonoBehaviour
{
    private void Start()
    {
        _ui_waitingLine = GameManager.UI.ShowScene<UI_WaitingLine>();
        _ui_turnCount = GameManager.UI.ShowScene<UI_TurnCount>();
        UI_hands = GameManager.UI.ShowScene<UI_Hands>();
        InitHands();
    }

    #region Turn Count
    private UI_TurnCount _ui_turnCount;
    private int _turnCount = 1;

    public void TurnPlus()
    {
        _turnCount++;
        _ui_turnCount.Refresh(_turnCount);
    }
    #endregion

    [SerializeField] private List<DeckUnit> _playerDeck = new List<DeckUnit>();
    public List<DeckUnit> PlayerDeck => _playerDeck;

    [SerializeField] private List<DeckUnit> _playerHands = new List<DeckUnit>();
    public List<DeckUnit> PlayerHands => _playerHands;
    public UI_Hands UI_hands;

    private void InitHands()
    {
        UI_hands.SetHands(PlayerHands);
    }

    public void AddDeckUnit(DeckUnit unit) {
        PlayerDeck.Add(unit);
    }

    public void RemoveDeckUnit(DeckUnit unit) {
        PlayerDeck.Remove(unit);
        UI_hands.RemoveUnit(unit);
    }

    public void AddHandUnit(DeckUnit unit)
    {
        PlayerHands.Add(unit);
        UI_hands.AddUnit(unit);
    }

    public void RemoveHandUnit(DeckUnit unit)
    {
        PlayerHands.Remove(unit);
        UI_hands.RemoveUnit(unit);
    }

    public DeckUnit GetRandomUnitFromDeck()
    {
        if (PlayerDeck.Count == 0)
        {
            return null;
        }
        int randNum = Random.Range(0, PlayerDeck.Count);

        DeckUnit unit = PlayerDeck[randNum];
        _playerDeck.RemoveAt(randNum);

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


    #region OrderedList
    private List<BattleUnit> _battleUnitOrderList = new List<BattleUnit>();
    private UI_WaitingLine _ui_waitingLine;

    public int OrderUnitCount => _battleUnitOrderList.Count;

    public void BattleUnitOrder()
    {
        _battleUnitOrderList.Clear();

        foreach (BattleUnit unit in BattleUnitList)
        {
            _battleUnitOrderList.Add(unit);
        }

        BattleOrderReplace();

        _ui_waitingLine.SetWaitingLine(_battleUnitOrderList);
    }

    private void BattleOrderReplace()
    {
        _battleUnitOrderList = _battleUnitOrderList.OrderByDescending(unit => unit.GetStat().SPD)
            .ThenByDescending(unit => unit.Location.y)
            .ThenBy(unit => unit.Location.x)
            .ToList();
    }

    public void BattleOrderRemove(BattleUnit removedUnit)
    {
        _ui_waitingLine.RemoveUnit(removedUnit);
        _battleUnitOrderList.Remove(removedUnit);
    }

    public BattleUnit GetNowUnit()
    {
        if (_battleUnitOrderList.Count > 0)
            return _battleUnitOrderList[0];
        return null;
    }
    #endregion
}
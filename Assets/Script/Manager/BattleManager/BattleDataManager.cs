using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDataManager : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    [SerializeField] private List<DeckUnit> _playerDeck = new();
    public List<DeckUnit> PlayerDeck => _playerDeck;

    [SerializeField] private List<DeckUnit> _playerHands = new();
    public List<DeckUnit> PlayerHands => _playerHands;

    public List<BattleUnit> HitUnits;
    public List<BattleUnit> CorruptUnits;

    // 전투를 진행중인 캐릭터가 들어있는 리스트
    private List<BattleUnit> _battleUnitList = new();
    public List<BattleUnit> BattleUnitList => _battleUnitList;

    private void Init()
    {
        _playerDeck = GameManager.Data.GetDeck().ToList<DeckUnit>();
        foreach (DeckUnit unit in _playerDeck)//UI에서 정보를 표시하기 전에 미리 할인함
        {
            unit.FirstTurnDiscount();
        }
    }

    public void OnBattleOver()
    {
        Debug.Log("BattleDataManager Destroy");

        foreach (BattleUnit unit in _battleUnitList)
        {
            unit.DeckUnit.DeckUnitChangedStat.ClearStat();
            AddDeckUnit(unit.DeckUnit);
            Debug.Log(unit.Data.Name);
        }

        _battleUnitList.Clear();

        foreach (DeckUnit unit in PlayerHands)
        {
            unit.DeckUnitChangedStat.ClearStat();
            AddDeckUnit(unit);
            Debug.Log(unit.Data.Name);
        }

        PlayerHands.Clear();

        GameManager.Data.SetDeck(_playerDeck);
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.SaveManager.SaveGame();
    }

    private int _turnCount = 0;
    public int TurnCount => _turnCount;

    public void TurnPlus()
    {
        _turnCount++;
        //BattleManager.BattleUI.UI_turnCount.Refresh();
    }

    public void DarkEssenseChage(int chage)
    {
        GameManager.Data.DarkEssenseChage(chage);
        BattleManager.BattleUI.UI_darkEssence.Refresh();
    }

    public void AddDeckUnit(DeckUnit unit) => PlayerDeck.Add(unit);

    public void RemoveDeckUnit(DeckUnit unit) => PlayerDeck.Remove(unit);

    public DeckUnit GetUnitFromDeck()
    {
        if (PlayerDeck.Count == 0)
        {
            return null;
        }
        DeckUnit unit = PlayerDeck[0];
        _playerDeck.RemoveAt(0);

        return unit;
    }

    public DeckUnit GetTUnitFromDeck()
    {
        if (PlayerDeck.Count == 0)
        {
            return null;
        }

        DeckUnit unit;

        if (PlayerDeck.Count == 4)
        {
            unit = PlayerDeck[3];
            _playerDeck.RemoveAt(3);
        }
        else
        {
            unit = PlayerDeck[0];
            _playerDeck.RemoveAt(0);
        }

        return unit;
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

    #region OrderedList
    private List<BattleUnit> _battleUnitOrderList = new();
    public int OrderUnitCount => _battleUnitOrderList.Count;

    public void BattleUnitOrder()
    {
        _battleUnitOrderList.Clear();

        foreach (BattleUnit unit in _battleUnitList)
        {
            _battleUnitOrderList.Add(unit);
        }

        BattleOrderReplace();

        BattleManager.BattleUI.RefreshWaitingLine(_battleUnitOrderList);
    }

    private void BattleOrderReplace()
    {
        _battleUnitOrderList = _battleUnitOrderList.OrderByDescending(unit => unit.BattleUnitTotalStat.SPD)
            .ThenByDescending(unit => unit.Location.y)
            .ThenBy(unit => unit.Location.x)
            .ToList();
    }

    public void BattleOrderRemove(BattleUnit removedUnit)
    {
        BattleManager.BattleUI.UI_waitingLine.RemoveUnit(removedUnit);
        _battleUnitOrderList.Remove(removedUnit);
        BattleManager.BattleUI.RefreshWaitingLine(_battleUnitOrderList);
    }

    public BattleUnit GetNowUnit()
    {
        if (_battleUnitOrderList.Count > 0)
            return _battleUnitOrderList[0];
        return null;
    }
    #endregion
}
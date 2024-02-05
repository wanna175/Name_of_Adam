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

    // 전투를 진행중인 캐릭터가 들어있는 리스트
    private List<BattleUnit> _battleUnitList = new();
    public List<BattleUnit> BattleUnitList => _battleUnitList;

    // 중복 타락을 처리하기 위한 팝업 리스트
    private List<UI_StigmaSelectButtonPopup> _corruptionPopups = new();
    public List<UI_StigmaSelectButtonPopup> CorruptionPopups => _corruptionPopups;

    [SerializeField] private BattleUnit incarnaUnit;
    public BattleUnit IncarnaUnit => incarnaUnit;

    public bool isDiscount = false;

    public bool isGameDone = false;

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
            if (unit.IsConnectedUnit || unit.Data.IsBattleOnly)
                continue;

            unit.DeckUnit.DeckUnitChangedStat.ClearStat();
            AddDeckUnit(unit.DeckUnit);
        }

        _battleUnitList.Clear();

        foreach (DeckUnit unit in PlayerHands)
        {
            unit.DeckUnitChangedStat.ClearStat();
            AddDeckUnit(unit);
        }

        PlayerHands.Clear();

        if (GameManager.OutGameData.IsUnlockedItem(8))
        {
            StageData data = GameManager.Data.Map.GetCurrentStage();
            if (data.StageLevel == 10)
            {
                foreach (DeckUnit unit in PlayerDeck)
                {
                    if (unit.DeckUnitStat.FallCurrentCount > 0)
                        unit.DeckUnitUpgradeStat.FallCurrentCount--;
                }
            }
        }

        GameManager.Data.SetDeck(_playerDeck);
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.OutGameData.SaveData();
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

    public DeckUnit GetTutorialUnitFromDeck()
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
    private List<(BattleUnit, int?)> _battleUnitOrderUnits = new();
    private List<BattleUnit> _battleUnitOrderList = new();
    public int OrderUnitCount => _battleUnitOrderList.Count;

    public void BattleUnitOrderReplace()
    {
        if (!BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
            return;

        _battleUnitOrderUnits.Clear();
        _battleUnitOrderList.Clear();

        foreach (BattleUnit unit in _battleUnitList)
        {
            if (unit.IsConnectedUnit ||
                unit.Data.UnitActionType == UnitActionType.UnitAction_None ||
                unit.Data.UnitActionType == UnitActionType.UnitAction_Horus_Egg
            )
                continue;

            _battleUnitOrderUnits.Add(new(unit, null));
            _battleUnitOrderList.Add(unit);
        }

        BattleUnitOrderSorting();
    }

    public void BattleUnitOrderSorting()
    {
        List<(BattleUnit, int?)> tempOrderList = new(_battleUnitOrderUnits);

        _battleUnitOrderList = _battleUnitOrderList.OrderByDescending(unit => {
            (BattleUnit, int?) result = tempOrderList.FirstOrDefault(item => item.Item1 == unit);

            if (result.Item2 == null)
            {
                tempOrderList.Remove(result);
                return unit.BattleUnitTotalStat.SPD;
            }
            else
            {
                tempOrderList.Remove(result);
                return result.Item2;
            }
        })
            .ThenBy(unit => unit.Team)
            .ThenByDescending(unit => unit.Location.y)
            .ThenBy(unit => unit.Location.x)
            .ToList();

        BattleManager.BattleUI.RefreshWaitingLine(_battleUnitOrderList);
    }

    public void BattleOrderRemove(BattleUnit removedUnit)
    {
        _battleUnitOrderList.Remove(removedUnit);
        BattleManager.BattleUI.RefreshWaitingLine(_battleUnitOrderList);
    }

    public void BattleOrderInsert(int index, BattleUnit addUnit, int? speed = null)
    {
        _battleUnitOrderList.Insert(index, addUnit);
        _battleUnitOrderUnits.Add(new(addUnit, speed));
        BattleManager.BattleUI.RefreshWaitingLine(_battleUnitOrderList);
    }

    public BattleUnit GetNowUnit()
    {
        if (_battleUnitOrderList.Count > 0)
            return _battleUnitOrderList[0];
        return null;
    }
    #endregion

    public UnitAction GetUnitAction(UnitActionType actionType)
    {
        if (actionType == UnitActionType.UnitAction)
        {
            return new UnitAction();
        }
        else if (actionType == UnitActionType.UnitAction_Laser)
        {
            return new UnitAction_Laser();
        }
        else if (actionType == UnitActionType.UnitAction_CenteredSplash)
        {
            return new UnitAction_CenteredSplash();
        }
        else if (actionType == UnitActionType.UnitAction_Iana)
        {
            return new UnitAction_Iana();
        }
        else if (actionType == UnitActionType.UnitAction_Phanuel)
        {
            return new UnitAction_Phanuel();
        }
        else if (actionType == UnitActionType.UnitAction_Appaim)
        {
            return new UnitAction_Appaim();
        }
        else if (actionType == UnitActionType.UnitAction_Tubalcain)
        {
            return new UnitAction_Tubalcain();
        }
        else if (actionType == UnitActionType.UnitAction_Horus)
        {
            return new UnitAction_Horus();
        }
        else if (actionType == UnitActionType.UnitAction_Horus_Egg)
        {
            return new UnitAction_Horus_Egg();
        }
        else if (actionType == UnitActionType.UnitAction_RaquelLeah)
        {
            return new UnitAction_RaquelLeah();
        }
        else
        {
            return new UnitAction_None();
        }
    }
}
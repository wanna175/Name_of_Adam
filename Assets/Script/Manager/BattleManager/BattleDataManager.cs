using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDataManager : MonoBehaviour
{
    private void Start()
    {
        UI_ControlBar UI_Control = GameManager.UI.ShowScene<UI_ControlBar>();
        GameManager.UI.ShowScene<UI_OptionButton>();

        _ui_waitingLine = GameManager.UI.ShowScene<UI_WaitingLine>();
        _ui_turnCount = GameManager.UI.ShowScene<UI_TurnCount>();

        UI_hands = UI_Control.UI_Hands;
        UI_PlayerSkill = UI_Control.UI_PlayerSkill;
        UI_DarkEssence = UI_Control.UI_DarkEssence;
        UI_ManaGauge = UI_Control.UI_ManaGauge;

        Init();
    }

    #region Turn Count
    private UI_TurnCount _ui_turnCount;
    private int _turnCount = 0;

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
    private int _maxHandCount = 3;

    public UI_Hands UI_hands;
    public UI_PlayerSkill UI_PlayerSkill;
    public UI_DarkEssence UI_DarkEssence;
    public UI_ManaGauge UI_ManaGauge;

    private void Init()
    {
        _playerDeck = GameManager.Data.GetDeck().ToList<DeckUnit>();
        Debug.Log(_playerDeck.Count);
        FillHand();
    }

    public void Destroy()
    {
        GameManager.Data.SetDeck(_playerDeck);
    }

    public void AddDeckUnit(DeckUnit unit) {
        PlayerDeck.Add(unit);
    }

    public void RemoveDeckUnit(DeckUnit unit) {
        PlayerDeck.Remove(unit);
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
        //FillHand();
        UI_hands.SetHands(GameManager.Data.GetDeck());
    }

    public void FillHand()
    {
        while (PlayerHands.Count < _maxHandCount)
        {
            DeckUnit unit = GetRandomUnitFromDeck();
            if (unit == null)
                return;
            AddHandUnit(unit);
        }
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
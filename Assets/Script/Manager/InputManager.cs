using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //UI들의 상호작용 관련 변수 관리
    [SerializeField] public Hands Hands;
    [SerializeField] public WatingLine WatingLine;
    
    #region Hands
    private int _ClickedHand = 0;
    public int ClickedHand => _ClickedHand;

    private DeckUnit _ClickedUnit = null;
    public DeckUnit ClickedUnit => _ClickedUnit;

    public void SetHand(int handIndex, DeckUnit unit)
    {
        _ClickedHand = handIndex;
        _ClickedUnit = unit;
    }

    public void ClearHand()
    {
        _ClickedHand = 0;
        _ClickedUnit = null;
    }

    #endregion

    private BattleUnit _SelectedUnit;
    public BattleUnit SelectedUnit
    {
        get { return _SelectedUnit; }
        set { _SelectedUnit = value; }
    }
}
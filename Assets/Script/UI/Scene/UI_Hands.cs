using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hands : UI_Scene
{
    [SerializeField] private GameObject HandPrefabs;
    [SerializeField] private Transform Grid;

    private List<UI_Hand> _handList = new List<UI_Hand>();
    
    public void SetHands(List<DeckUnit> deckUnits)
    {
        foreach (DeckUnit unit in deckUnits)
            AddUnit(unit);
    }

    public void AddUnit(DeckUnit unit)
    {
        UI_Hand newCard = GameObject.Instantiate(HandPrefabs, Grid).GetComponent<UI_Hand>();
        newCard.SetHandUnit(unit);
        _handList.Add(newCard);
    }

    public void RemoveUnit(DeckUnit unit)
    {
        UI_Hand card = FindCardByUnit(unit);

        if (card != null)
            DestroyCard(card);
    }

    public void ClearHands()
    {
        foreach (UI_Hand card in _handList)
            DestroyCard(card);
    }

    private void DestroyCard(UI_Hand card)
    {
        _handList.Remove(card);
        Destroy(card.gameObject);
    }

    private UI_Hand FindCardByUnit(DeckUnit unit)
    {
        foreach(UI_Hand card in _handList)
        {
            if (card.GetHandUnit() == unit)
                return card;
        }

        return null;
    }

    public DeckUnit GetSelectedUnit()
    {
        foreach (UI_Hand hand in _handList)
            if (hand.IsSelected)
                return hand.GetHandUnit();
        return null;
    }
    #region Hand Click
    private int _ClickedHand = 0;
    public int ClickedHand => _ClickedHand;

    private DeckUnit _ClickedUnit = null;
    public DeckUnit ClickedUnit => _ClickedUnit;

    public void OnHandClick(UI_Hand hand)
    {
        _ClickedHand = _handList.IndexOf(hand);
        _ClickedUnit = hand.GetHandUnit();

        // Memo : 소환 이전에 클릭이 되지 않아야 함
        //if (!_Data.Mana.CanUseMana(_ClickedUnit.Data.ManaCost)){
        //    Debug.Log("not enough mana");
        //    ClearHand();
        //}
    }

    public void ClearHand()
    {
        _ClickedHand = 0;
        _ClickedUnit = null;
    }
    #endregion
}

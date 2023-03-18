using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hands : UI_Scene
{
    [SerializeField] private GameObject HandPrefabs;
    [SerializeField] private Transform Grid;

    private List<UI_Hand> _handList = new List<UI_Hand>();
    private UI_Hand _selectedHand = null;
    private BattleManager _battle = GameManager.Battle;
    
    public void SetHands(List<DeckUnit> deckUnits)
    {
        foreach (DeckUnit unit in deckUnits)
            AddUnit(unit);
    }

    public void AddUnit(DeckUnit unit)
    {
        UI_Hand newCard = GameObject.Instantiate(HandPrefabs, Grid).GetComponent<UI_Hand>();
        newCard.SetUnit(this, unit);
        _handList.Add(newCard);
    }

    public void RemoveUnit(DeckUnit unit)
    {
        UI_Hand card = FindCardByUnit(unit);

        if (card != null)
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
            if (card.GetUnit() == unit)
                return card;
        }

        return null;
    }

    public void OnClickHand(UI_Hand hand)
    {
        Debug.Log("Hand Click");
        if (hand != null && hand == _selectedHand)
            CancleSelect();
        else
            SelectOneUnit(hand);
    }

    private void CancleSelect()
    {
        _selectedHand.ChangeSelectState(false);
        _selectedHand = null;
        _battle.UnitSpawnReady(false);
    }

    private void SelectOneUnit(UI_Hand hand)
    {
        if (_selectedHand != null)
            _selectedHand.ChangeSelectState(false);
        
        _selectedHand = hand;
        _selectedHand.ChangeSelectState(true);
        _battle.UnitSpawnReady(true);
    }

    public DeckUnit GetSelectedUnit()
    {
        return _selectedHand.GetUnit();
    }
}

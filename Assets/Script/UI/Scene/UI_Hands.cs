using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hands : UI_Scene
{
    [SerializeField] private GameObject HandPrefabs;
    [SerializeField] private Transform Grid;

    private List<UI_Hand> _handList = new();
    public UI_Hand _selectedHand = null;

    public void AddUnit(DeckUnit unit)
    {
        UI_Hand newCard = GameObject.Instantiate(HandPrefabs, Grid).GetComponent<UI_Hand>();
        newCard.SetUnit(this, unit);
        _handList.Add(newCard);
    }

    public void RemoveUnit(DeckUnit unit)
    {
        UI_Hand card = FindCardByUnit(unit);
        card.ChangeSelectState(false);

        if (card != null)
            DestroyCard(card);
    }

    private void DestroyCard(UI_Hand card)
    {
        _handList.Remove(card);
        Destroy(card.gameObject);
    }

    public void RefreshCard()
    {
        foreach (UI_Hand card in _handList)
        {
            card.SetUnitInfo();
        }
     }

    public UI_Hand FindCardByUnit(DeckUnit unit)
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
        if (BattleManager.Mana.CanUseMana(hand.GetUnit().DeckUnitTotalStat.ManaCost) && GameManager.Data.CanUseDarkEssense(hand.GetUnit().Data.DarkEssenseCost))
        {
            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

            if (BattleManager.BattleUI.UI_playerSkill._selectedCard != null)
                return;
            else if (hand != null && hand == _selectedHand)
                CancelSelect();
            else
                SelectOneUnit(hand);
        }
        else
        {
            GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX"); 
            Debug.Log("Can't");
        }
    }

    public void CancelSelect()
    {
        if (_selectedHand == null)
            return;

        _selectedHand.ChangeSelectState(false);
        _selectedHand = null;
        BattleManager.Instance.UnitSpawnReady(FieldColorType.none);
    }

    private void SelectOneUnit(UI_Hand hand)
    {
        if (_selectedHand != null)
            _selectedHand.ChangeSelectState(false);
        
        _selectedHand = hand;
        _selectedHand.ChangeSelectState(true);

        BattleManager.Instance.UnitSpawnReady(FieldColorType.none);
        BattleManager.Instance.UnitSpawnReady(FieldColorType.UnitSpawn, hand.GetUnit());
    }

    public DeckUnit GetSelectedUnit() =>_selectedHand.GetUnit();
    
    public void InableCheck(int manaValue)
    {
        foreach (UI_Hand card in _handList)
        {
            if (manaValue < card.GetUnit().DeckUnitTotalStat.ManaCost)
            {
                card.ChangeInable(true);
            }
            else
            {
                card.ChangeInable(false);
            }
        }
    }
}

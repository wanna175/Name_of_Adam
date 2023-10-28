using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MyDeck : UI_Popup
{
    [SerializeField] private GameObject CardPrefabs;
    [SerializeField] private Transform Grid;
    private bool _select; //UnitInfo에 전달용
    private List<DeckUnit> _playerDeck = new();
    private List<DeckUnit> _hallDeck = new();
    private Action<DeckUnit> _onSelect;

    public void Init(bool battle=false, Action<DeckUnit> onSelect=null)
    {
        if (battle)
            _playerDeck = BattleManager.Data.PlayerDeck;
        else
            _playerDeck = GameManager.Data.GetDeck();

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }

    public void HallDeckInit(bool isElite = false, Action<DeckUnit> onSelect = null)
    {
        List<DeckUnit> _eliteDeck = new();
        List<DeckUnit> _normalDeck = new();

        _hallDeck = GameManager.Data.GetDeck();

        foreach (DeckUnit unit in _hallDeck)
        {
            if (unit.Data.Rarity == Rarity.일반)
            {
                _normalDeck.Add(unit);
            }
            else
            {
                _eliteDeck.Add(unit);
            }
        }

        if (isElite)
        {
            _playerDeck = _eliteDeck;
        }
        else
        {
            _playerDeck = _normalDeck;
        }

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }

    public void SetCard()
    {
        foreach (DeckUnit unit in _playerDeck)
        {
            AddCard(unit);
        }
    }

    public void AddCard(DeckUnit unit)
    {
        UI_Card newCard = GameObject.Instantiate(CardPrefabs, Grid).GetComponent<UI_Card>();
        newCard.SetCardInfo(this, unit);
    }

    public void OnClickCard(DeckUnit unit)
    {
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(unit);
        ui.Init(_onSelect);
    }

    public void Quit()
    {
        //GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        GameManager.UI.ClosePopup();
    }
}

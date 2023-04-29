using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MyDeck : UI_Popup
{
    private List<DeckUnit> _playerDeck = new();
    [SerializeField] private GameObject CardPrefabs;
    [SerializeField] private Transform Grid;
    private bool _select; //UnitInfo에 전달용

    public void Init(bool battle=false, bool select=false)
    {
        if (battle)
            _playerDeck = BattleManager.Data.PlayerDeck;
        else
            _playerDeck = GameManager.Data.GetDeck();

        _select = select;

        SetCard();

        //GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
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
        newCard.SetCardInfo(unit, _select);
    }

    public void Quit()
    {
        GameManager.UI.ClosePopup();
    }
}

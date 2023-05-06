using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MyDeck : UI_Popup
{
    [SerializeField] private GameObject CardPrefabs;
    [SerializeField] private Transform Grid;
    private bool _select; //UnitInfo에 전달용
    private List<DeckUnit> _playerDeck = new();
    private Selectable _selectable;

    public void Init(bool battle=false, bool select=false, Selectable selectable=null)
    {
        if (battle)
            _playerDeck = BattleManager.Data.PlayerDeck;
        else
            _playerDeck = GameManager.Data.GetDeck();

        if (select)
        {
            _select = select;
            _selectable = selectable;
        }


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
        newCard.SetCardInfo(this, unit);
    }

    public void OnClickCard(DeckUnit unit)
    {
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(unit);
        ui.Init(_select, _selectable);
    }

    public void Quit()
    {
        GameManager.UI.ClosePopup();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MyDeck : UI_Popup
{
    private List<DeckUnit> _playerDeck = new();
    [SerializeField] private GameObject CardPrefabs;
    [SerializeField] private Transform Grid;

    private List<UI_Card> _cardList = new List<UI_Card>();

    public void Start() 
    {
        _playerDeck = GameManager.Data.GetDeck();
        //GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
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
        newCard.SetUnit(unit);
        _cardList.Add(newCard);
    }

    public void Quit()
    {
        GameManager.UI.ClosePopup();
    }
}

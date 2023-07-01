using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Card : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private UI_UnitCard _unitCard;

    private UI_MyDeck _myDeck;
    private DeckUnit _cardUnit = null;
    
    private void Start()
    {
        _highlight.SetActive(false);
    }

    public void SetCardInfo(UI_MyDeck myDeck, DeckUnit unit)
    {
        _myDeck = myDeck;
        _cardUnit = unit;

        _unitCard.Set(_cardUnit.Data.Image, _cardUnit.Data.Name, _cardUnit.DeckUnitTotalStat.ManaCost.ToString());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _myDeck.OnClickCard(_cardUnit);
    }
}

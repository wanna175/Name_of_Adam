using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Card : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private UI_MyDeck _myDeck;
    private DeckUnit _cardUnit = null;
    [SerializeField] private GameObject _highlight;
    private bool _select; //UnitInfo에 전달용
    
    private void Start()
    {
        _highlight.SetActive(false);
    }

    public void SetCardInfo(UI_MyDeck myDeck, DeckUnit unit)
    {
        _myDeck = myDeck;
        _cardUnit = unit;

        // UI가 완성된 후에 디테일한 요소 추가
        GetComponent<Image>().sprite = _cardUnit.Data.Image;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Card : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _disable;
    [SerializeField] private Image _unitImage;
    [SerializeField] private TextMeshProUGUI _name;

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

        _unitImage.sprite = unit.Data.Image;
        _name.text = unit.Data.Name;

        if(SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            SetDisable(unit);
        }
    }

    public void SetDisable(DeckUnit unit)
    {
        _disable.SetActive(unit.IsMainDeck);
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
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (_disable.activeSelf)
        {
            return;
        }

        _myDeck.OnClickCard(_cardUnit);
    }
}

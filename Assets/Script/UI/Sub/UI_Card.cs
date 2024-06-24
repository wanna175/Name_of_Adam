using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UI_Card : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _selectHighlight;
    [SerializeField] private GameObject _disable;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private List<GameObject> _stigmataFrames;

    [SerializeField] private Sprite _normalFrame;
    [SerializeField] private Sprite _eliteFrame;

    [SerializeField] private List<Image> _stigmataImages;

    private UI_MyDeck _myDeck;
    private DeckUnit _cardUnit = null;

    private Action _disableClickAction;

    private void Start()
    {
        _highlight.SetActive(false);
        _selectHighlight.SetActive(false);
    }

    public void SetCardInfo(UI_MyDeck myDeck, DeckUnit unit)
    {
        _myDeck = myDeck;
        _cardUnit = unit;

        _unitImage.sprite = unit.Data.CorruptImage;
        _name.text = unit.Data.Name;

        if(unit.Data.Rarity == Rarity.Normal)
        {
            _frameImage.sprite = _normalFrame;
        }
        else
        {
            _frameImage.sprite = _eliteFrame;
        }

        if (SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            if (unit.IsMainDeck)
                SetDisable(() => GameManager.UI.ShowPopup<UI_SystemInfo>().Init("TeamBuild_MainDeck", string.Empty));
        }

        foreach (var frame in _stigmataFrames)
            frame.SetActive(true);

        List<Stigma> unitStigmaList = unit.GetStigma();
        for (int i = 0; i < _stigmataImages.Count; i++)
        {
            if (i < unitStigmaList.Count)
            {
                _stigmataFrames[i].GetComponent<UI_StigmaHover>().SetStigma(unitStigmaList[i]);
                _stigmataImages[i].sprite = unitStigmaList[i].Sprite_28;
                _stigmataImages[i].color = Color.white;
            }
            else
            {
                _stigmataFrames[i].GetComponent<UI_StigmaHover>().SetEnable(false);
                _stigmataImages[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    public void SelectCard()
    {
        _selectHighlight.SetActive(!_selectHighlight.activeInHierarchy);
    }

    public void SetDisable(Action disableClickAction)
    {
        _disable.SetActive(true);
        _disableClickAction = disableClickAction;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_cardUnit.IsMainDeck && SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            return;
        }
        else
        {
            _highlight.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_cardUnit.IsMainDeck && SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            return;
        }
        else
        {
            _highlight.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (_disable.activeSelf)
            _disableClickAction();
        else
            _myDeck.OnClickCard(_cardUnit);
    }
}

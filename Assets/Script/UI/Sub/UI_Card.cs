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
    [SerializeField] private GameObject _selectHighlight;
    [SerializeField] private GameObject _disable;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField]
    private List<GameObject> _stigmaFrames;

    private List<Image> _stigmaImages;

    public Sprite NormalFrame;
    public Sprite EliteFrame;
    private UI_MyDeck _myDeck;
    private DeckUnit _cardUnit = null;
    private CUR_EVENT evNum = CUR_EVENT.NONE;

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
            _frameImage.sprite = NormalFrame;
        }
        else
        {
            _frameImage.sprite = EliteFrame;
        }

        if(SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            SetDisableMain(unit);
        }

        _stigmaImages = new List<Image>();
        foreach (var frame in _stigmaFrames)
            _stigmaImages.Add(frame.GetComponentsInChildren<Image>()[1]);

        foreach (var frame in _stigmaFrames)
            frame.SetActive(true);

        List<Stigma> stigmas = unit.GetStigma();
        for (int i = 0; i < _stigmaImages.Count; i++)
        {
            if (i < stigmas.Count)
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetStigma(stigmas[i]);
                _stigmaImages[i].sprite = stigmas[i].Sprite_28;
                _stigmaImages[i].color = Color.white;
            }
            else
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetEnable(false);
                _stigmaImages[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }
    public void SelectCard()
    {
        if (!_selectHighlight.activeInHierarchy)
            this._selectHighlight.SetActive(true);
        else
            this._selectHighlight.SetActive(false);
    }

    public void SetDisableMain(DeckUnit unit)
    {
        _disable.SetActive(unit.IsMainDeck);
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
        {
            return;
        }

        _myDeck.OnClickCard(_cardUnit);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_HallCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] 
    private GameObject _highlight;

    [SerializeField]
    private List<GameObject> _stigmaFrames;

    private List<Image> _stigmaImages;

    [SerializeField]
    private TMP_Text _nameText;

    [SerializeField]
    private GameObject infoButton;

    public Image UnitImage;
    private List<DeckUnit> _mainDeck;
    private List<HallUnit> _hallUnitList;

    private bool _isEnable;
    public bool _isElite;
    public int HallUnitID; //덱과 대칭시킬 ID

    void Start()
    {
        Init();
    }

    public void Init()
    {
        _hallUnitList = GameManager.OutGameData.FindHallUnitList();
        _mainDeck = GameManager.Data.GameDataMain.DeckUnits;

        _stigmaImages = new List<Image>();
        foreach (var frame in _stigmaFrames)
            _stigmaImages.Add(frame.GetComponentsInChildren<Image>()[1]);

        if (_mainDeck.Count <= HallUnitID)
        {
            _isEnable = false;
            DisableUI();
            return;
        }

        foreach (var frame in _stigmaFrames)
            frame.SetActive(true);

        _isEnable = true;
        UnitImage.sprite = _mainDeck[HallUnitID].Data.Image;
        UnitImage.color = Color.white;
        _nameText.SetText(_mainDeck[HallUnitID].Data.name);
        infoButton.SetActive(true);

        List<Stigma> stigmas = _mainDeck[HallUnitID].GetStigma();
        for (int i = 0; i < _stigmaImages.Count; i++)
        {
            if (i < stigmas.Count)
            {
                _stigmaImages[i].sprite = stigmas[i].Sprite_28;
                _stigmaImages[i].color = Color.white;
            }
            else
            {
                _stigmaImages[i].color = new Color(1f, 1f, 1f, 0f);            
            }
        }

        _highlight.SetActive(false);
    }

    private void DisableUI()
    {
        UnitImage.color = new Color(1f, 1f, 1f, 0f);
        _nameText.SetText("");
        infoButton.SetActive(false);
        foreach (var frame in _stigmaFrames)
            frame.SetActive(false);
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallDeckInit(_isElite, OnSelect);
    }

    //선택한 유닛 GameDataMain 에 저장되게 하기
    public void OnSelect(DeckUnit unit)
    {
        if (_mainDeck.Count <= HallUnitID)
        {
            GameManager.Data.GameDataMain.DeckUnits.Add(unit);
        }

        _mainDeck[HallUnitID].IsMainDeck = false;
        _hallUnitList[_mainDeck[HallUnitID].HallUnitID].IsMainDeck = false;
        _mainDeck[HallUnitID] = unit;
        _mainDeck[HallUnitID].IsMainDeck = true;
        _hallUnitList[_mainDeck[HallUnitID].HallUnitID].IsMainDeck = true;

        foreach (Stigma stigma in unit.GetStigma())
        {
            _mainDeck[HallUnitID].AddStigma(stigma);
        }

        UnitImage.sprite = _mainDeck[HallUnitID].Data.Image;
        UnitImage.color = Color.white;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();

        Init();
    }

    public void OnInfoButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(_mainDeck[HallUnitID]);
        ui.Init();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isEnable)
            _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isEnable)
            _highlight.SetActive(false);
    }
}

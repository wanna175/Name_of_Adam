using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_HallCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _infoButton;
    [SerializeField] private List<GameObject> _frameList;
    [SerializeField] private TMP_Text _nameText;

    [SerializeField] private Image _unitImage;

    [SerializeField] private List<GameObject> _stigmataFrame;
    [SerializeField] private List<Image> _stigmataImage;

    [SerializeField] private GameObject _highlight;

    [SerializeField] public int _hallSlotID;


    private List<HallUnit> _hallUnitList;
    private List<DeckUnit> _mainDeck;

    private List<Image> _stigmaImages;


    private DeckUnit _deckUnit;

    private bool _isEnable;
    private bool _isElite;


    public void Init()
    {
        _hallUnitList = GameManager.OutGameData.FindHallUnitList();
        _mainDeck = GameManager.Data.GameDataMain.DeckUnits;
        _mainDeck.Sort((x, y) => x.HallUnitID.CompareTo(y.HallUnitID));

        _deckUnit = _mainDeck.Find(x => x.HallUnitID == _hallSlotID);
        if (_deckUnit == null)
        {
            _isEnable = false;
            DisableUI();
            return;
        }

        _isEnable = true;

        _frameList[0].SetActive(_deckUnit.Data.Rarity == Rarity.Normal);
        _frameList[1].SetActive(_deckUnit.Data.Rarity != Rarity.Normal);

        _unitImage.sprite = _deckUnit.Data.CorruptImage;
        _unitImage.gameObject.SetActive(true);

        _nameText.SetText(_deckUnit.Data.Name);
        _infoButton.SetActive(true);

        List<Stigma> stigmaList = _deckUnit.GetStigma();
        for (int i = 0; i < _stigmataImage.Count; i++)
        {
            if (i < stigmaList.Count)
            {
                _stigmataFrame[i].GetComponent<UI_StigmaHover>().SetStigma(stigmaList[i]);
                _stigmataImage[i].sprite = stigmaList[i].Sprite_28;
                _stigmataImage[i].gameObject.SetActive(true);
            }
            else
            {
                _stigmataFrame[i].GetComponent<UI_StigmaHover>().SetEnable(false);
                _stigmataImage[i].gameObject.SetActive(false);
            }
        }

        if (GameManager.OutGameData.IsUnlockedItem(14) == false)
        {
            if (_hallSlotID == 1 || _hallSlotID == 2)
            {
                _isEnable = false;
            }
        }

        _highlight.SetActive(false);
    }

    private void DisableUI()
    {
        _unitImage.gameObject.SetActive(false);
        _nameText.SetText("");
        _infoButton.SetActive(false);
        foreach (var frame in _stigmataFrame)
        {
            frame.SetActive(false);
        }
    }

    public void OnClick()
    {
        if ((_hallSlotID == 1 || _hallSlotID == 2) && !GameManager.OutGameData.IsUnlockedItem(14))
        {
            GameManager.Sound.Play("UI/UISFX/UIFailSFX");
            return;
        }

        GameManager.Sound.Play("UI/UISFX/UISelectSFX");

        if (GameManager.OutGameData.IsUnlockedItem(17))
        {
            GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallEliteDeckInit(_isElite, OnSelect);
        }
        else
        {
            GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallDeckInit(_isElite, OnSelect);
        }
    }

    //������ ���� GameDataMain �� ����ǰ� �ϱ�
    public void OnSelect(DeckUnit unit)
    {
        DeckUnit afterDeckUnit = unit;
        HallUnit afterHallUnit = _hallUnitList.Find(x => x.ID == afterDeckUnit.HallUnitID);

        if (_mainDeck.Find(x => x.HallUnitID == _hallSlotID) == null)
        {
            // 신규 유닛이면 추가
            if (_mainDeck.Count < _hallSlotID)
                _mainDeck.Add(unit);
            else
                _mainDeck.Insert(_hallSlotID, unit);
        }
        else
        {
            // 이전 유닛이 있다면 스왑
            DeckUnit beforeDeckUnit = GameManager.Data.GetDeck().Find(x => x.HallUnitID == _hallSlotID);
            HallUnit beforeHallUnit = _hallUnitList.Find(x => x.ID == beforeDeckUnit.HallUnitID);

            beforeDeckUnit.IsMainDeck = false;
            beforeHallUnit.IsMainDeck = false;
            beforeDeckUnit.HallUnitID = afterDeckUnit.HallUnitID;
            beforeHallUnit.ID = afterHallUnit.ID;

            _mainDeck[_hallSlotID] = unit;
        }

        // GameData Deck 수정
        afterDeckUnit.IsMainDeck = true;
        afterHallUnit.IsMainDeck = true;
        afterDeckUnit.HallUnitID = _hallSlotID;
        afterHallUnit.ID = _hallSlotID;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();

        Init();
    }

    public void OnInfoButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(_mainDeck[_hallSlotID]);
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

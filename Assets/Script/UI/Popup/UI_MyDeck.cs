using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_MyDeck : UI_Popup
{
    [SerializeField] private GameObject _cardPrefabs;
    [SerializeField] private Transform _grid;
    [SerializeField] private GameObject _quitButton;//종료버튼
    [SerializeField] private GameObject _setButton;//결정 버튼
    [SerializeField] private TextMeshProUGUI _quit_txt;//제목 텍스트
    [SerializeField] private TextMeshProUGUI _title_txt;//제목 텍스트
    [SerializeField] private TMP_Text _pageText;
    [SerializeField] private GameObject _prePageButton;
    [SerializeField] private GameObject _postPageButton;

    private List<DeckUnit> _playerDeck = new();
    private List<DeckUnit> _hallDeck = new();
    private Dictionary<DeckUnit, UI_Card> _card_dic = new();//선택된 유닛
    private Action<DeckUnit> _onSelect;
    private Action _endEvent;
    private CUR_EVENT _eventNum = CUR_EVENT.NONE;
    private bool _isBossClear;
    private GameObject _eventMenu = null;
    private int _currentPageIndex;
    private int _maxPageIndex;

    public void Init(bool battle = false, Action<DeckUnit> onSelect = null, CUR_EVENT eventNum = CUR_EVENT.NONE, Action endEvent=null)
    {
        _setButton.SetActive(false);

        if (eventNum == CUR_EVENT.RECEIVE_STIGMA)
            _quitButton.SetActive(false);

        if (battle)
            _playerDeck = BattleManager.Data.PlayerDeck;
        else
            _playerDeck = GameManager.Data.GetDeck();

        _currentPageIndex = 0;
        _maxPageIndex = (_playerDeck.Count - 1) / 10;
        
        if (_maxPageIndex < 0)
            _maxPageIndex = 0;

        ClearCard();
        SetPageAllUI();

        if (onSelect != null)
            _onSelect = onSelect;
        if (endEvent != null)
            _endEvent = endEvent;

        _eventNum = eventNum;

        EventSceneCheck(_eventNum);

        if (_eventNum == CUR_EVENT.GIVE_STIGMA || _eventNum == CUR_EVENT.STIGMA)
            SetCard(_eventNum);
        else
            SetCard();
    }

    public void HallSaveInit(bool isBossClear, Action<DeckUnit> onSelect = null)
    {
        _quit_txt.text = GameManager.Locale.GetLocalizedEventScene("Skip");
        _isBossClear = isBossClear;

        List<DeckUnit> normalDeck = new();
        List<DeckUnit> totalDeck = new();
        _hallDeck = GameManager.Data.GameData.FallenUnits;

        _currentPageIndex = 0;
        _maxPageIndex = (_hallDeck.Count - 1) / 10;
        if (_maxPageIndex < 0)
            _maxPageIndex = 0;

        ClearCard();
        SetPageAllUI();

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to bring to the Divine Hall.");

        foreach (DeckUnit unit in _hallDeck)
        {
            totalDeck.Add(unit);

            if (unit.Data.Rarity == Rarity.Normal)
            {
                normalDeck.Add(unit);
            }
        }

        if (isBossClear)
        {
            _playerDeck = totalDeck;
        }
        else
        {
            _playerDeck = normalDeck;
        }

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }

    public void HallDeckInit(bool isBoss = false, Action<DeckUnit> onSelect = null)
    {
        List<DeckUnit> eliteDeck = new();
        List<DeckUnit> normalDeck = new();

        _hallDeck = GameManager.Data.GetDeck();

        _currentPageIndex = 0;
        _maxPageIndex = (_hallDeck.Count - 1) / 10;
        if (_maxPageIndex < 0)
            _maxPageIndex = 0;

        ClearCard();
        SetPageAllUI();

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to bring to the Divine Hall.");

        foreach (DeckUnit unit in _hallDeck)
        {
            if (unit.Data.Rarity == Rarity.Normal)
            {
                normalDeck.Add(unit);
            }
            else
            {
                eliteDeck.Add(unit);
            }
        }

        if (isBoss)
        {
            _playerDeck = eliteDeck;
        }
        else
            _playerDeck = normalDeck;

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }

    public void HallEliteDeckInit(bool isBoss = false, Action<DeckUnit> onSelect = null)
    {
        List<DeckUnit> eliteDeck = new();

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to bring to the Divine Hall.");

        _hallDeck = GameManager.Data.GetDeck();

        _currentPageIndex = 0;
        _maxPageIndex = (_playerDeck.Count - 1) / 10;
        if (_maxPageIndex < 0)
            _maxPageIndex = 0;

        ClearCard();
        SetPageAllUI();

        foreach (DeckUnit unit in _hallDeck)
        {
            if (unit.Data.Rarity != Rarity.Boss)
            {
                eliteDeck.Add(unit);
            }
        }

        if (isBoss)
        {
            _playerDeck = _hallDeck;
        }
        else
        {
            _playerDeck = eliteDeck;
        }

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }

    public void SetCard() 
    {
        ClearCard();

        for (int i = 10 * _currentPageIndex; i < (_currentPageIndex + 1) * 10; i++)
        {
            if (i >= _playerDeck.Count)
                break;

            AddCard(_playerDeck[i]);
        }
    }

    private void SetCard(CUR_EVENT eventNum)
    {
        ClearCard();

        for (int i = 10 * _currentPageIndex; i < (_currentPageIndex + 1) * 10; i++)
        {
            if (i >= _playerDeck.Count)
                break;

            if (eventNum == CUR_EVENT.GIVE_STIGMA)
            {
                if (_playerDeck[i].GetStigma(true).Count != 0)
                    AddCard(_playerDeck[i]);
            }
            else if (eventNum == CUR_EVENT.STIGMA)
            {
                AddCard(_playerDeck[i]);
                _card_dic[_playerDeck[i]].SetDisable(_playerDeck[i].Data.Rarity == Rarity.Boss);
            }

        }
    }

    private void ClearCard()
    {
        var dumpCards = _grid.GetComponentsInChildren<UI_Card>();
        foreach (var card in dumpCards)
            GameManager.Resource.Destroy(card.gameObject);
    }

    public void AddCard(DeckUnit unit)
    {
        UI_Card newCard = GameObject.Instantiate(_cardPrefabs, _grid).GetComponent<UI_Card>();
        newCard.SetCardInfo(this, unit);

        _card_dic[unit] = newCard;
    }

    public void OnClickCard(DeckUnit unit)
    {
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");
        ui.SetUnit(unit);

        if (_eventNum == CUR_EVENT.HARLOT_RESTORATION)
            ui.Restoration(_onSelect, _eventNum, OnSelectRestorationUnit);
        else
            ui.Init(_onSelect, _eventNum);
    }

    public void OnSelectRestorationUnit(DeckUnit unit)
    {
        _card_dic[unit].SelectCard();
    }

    //���� ȯ�� �� ������ư...
    public void SetButtonClick()
    {
        _endEvent.Invoke();
    }

    public void SetEventMenu(GameObject obj)
    {
        _eventMenu = obj;
    }

    private void EventSceneCheck(CUR_EVENT EventScene)
    {
        string sceneName = currentSceneName();
        if (sceneName.Equals("EventScene"))
        {
            //Quit_btn.SetActive(false);

            if (EventScene == CUR_EVENT.UPGRADE)
                _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to upgrade.");
            else if (EventScene == CUR_EVENT.RELEASE)
                _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to restore faith.");
            else if (EventScene == CUR_EVENT.STIGMA || EventScene == CUR_EVENT.RECEIVE_STIGMA)
                _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to bestow stigmata.");
            else if (EventScene == CUR_EVENT.GIVE_STIGMA)
                _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to sacrifice.");
            else if (EventScene == CUR_EVENT.HARLOT_RESTORATION)
            {
                _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select units to revert.");
                _setButton.SetActive(true);
            }
        }
        else
        {
            _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Possessed units");
        }
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");

        if (_quit_txt.text == "Skip" || _quit_txt.text == "선택 안함")
        {
            if (_isBossClear)
            {
                SceneChanger.SceneChange("EndingCreditScene");
            }
            else
            {
                SceneChanger.SceneChange("MainScene");
            }
        }
        else
        {
            if (_eventMenu != null)
                _eventMenu.SetActive(true);

            GameManager.UI.ClosePopup();
        }
    }

    private void SetPageAllUI()
    {
        SetPageText();
        SetPreButtonUI();
        SetPostButtonUI();
    }

    private void SetPageText()
    {
        if (_maxPageIndex == 0)
            _pageText.SetText("");
        else
            _pageText.SetText($"( {_currentPageIndex + 1} / {_maxPageIndex + 1} )");
    }

    private void SetPreButtonUI()
    {
        if (_currentPageIndex == 0)
            _prePageButton.SetActive(false);
        else
            _prePageButton.SetActive(true);
    }

    private void SetPostButtonUI()
    {
        if (_currentPageIndex == _maxPageIndex)
            _postPageButton.SetActive(false);
        else
            _postPageButton.SetActive(true);
    }

    public void OnPrePageButton()
    {
        _currentPageIndex--;
        SetCard();
        SetPageAllUI();
    }

    public void OnPostPageButton()
    {
        _currentPageIndex++;
        SetCard();
        SetPageAllUI();
    }
}
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
    [SerializeField] private GameObject _myDeckButton;

    private List<DeckUnit> _playerDeck = new();
    private List<DeckUnit> _hallDeck = new();
    private Dictionary<DeckUnit, UI_Card> _card_dic = new();//선택된 유닛

    private Action<DeckUnit> _onSelectAction;
    private CurrentEvent _currentEvent = CurrentEvent.None;
    private GameObject _eventMenu = null;
    private Action _endEvent;

    private bool _isBossClear;
    private int _currentPageIndex;
    private int _maxPageIndex;

    public void Init(bool isDeckButtonClick = false)
    {
        _setButton.SetActive(false);
        _myDeckButton.SetActive(isDeckButtonClick);//내 덱 보기 버튼으로 진입했을 경우

        _currentPageIndex = 0;
        _maxPageIndex = Mathf.Max((_playerDeck.Count - 1) / 10, 0);

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Possessed units");

        _playerDeck = GameManager.Data.GetDeck();

        SetPageAllUI();
        SetCard();
    }

    public void EventInit(Action<DeckUnit> onSelectAction = null, CurrentEvent currentEvent = CurrentEvent.None, GameObject eventMenu = null, Action endEvent = null)
    {
        _currentEvent = currentEvent;
        _onSelectAction = onSelectAction;
        _eventMenu = eventMenu;
        _endEvent = endEvent;

        string titleText = "";

        if (_currentEvent == CurrentEvent.Upgrade_Select)
        {
            titleText = "Select a unit to upgrade.";
        }
        else if (_currentEvent == CurrentEvent.Heal_Faith_Select)
        {
            titleText = "Select a unit to restore faith.";
        }
        else if (_currentEvent == CurrentEvent.Stigmata_Select)
        {
            titleText = "Select a unit to bestow stigmata.";
        }
        else if (_currentEvent == CurrentEvent.Stigmata_Receive)
        {
            titleText = "Select a unit to bestow stigmata.";
            _quitButton.SetActive(false);
        }
        else if (_currentEvent == CurrentEvent.Stigmata_Give)
        {
            titleText = "Select a unit to sacrifice.";
        }
        else if (_currentEvent == CurrentEvent.Unit_Restoration_Select)
        {
            titleText = "Select units to revert.";
            _setButton.SetActive(true);
        }

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene(titleText);

        SetCard(_currentEvent);
    }

    public void HallSaveInit(bool isBossClear, Action<DeckUnit> onSelectAction = null)
    {
        _quit_txt.text = GameManager.Locale.GetLocalizedEventScene("Skip");
        _isBossClear = isBossClear;

        List<DeckUnit> normalDeck = new();
        List<DeckUnit> totalDeck = new();
        _hallDeck = GameManager.Data.GameData.FallenUnits;

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

        _onSelectAction = onSelectAction;

        _currentPageIndex = 0;
        _maxPageIndex = (_playerDeck.Count - 1) / 10;
        if (_maxPageIndex < 0)
            _maxPageIndex = 0;

        ClearCard();
        SetCard();
        SetPageAllUI();
    }

    public void HallDeckInit(bool isBoss = false, Action<DeckUnit> onSelectAction = null)
    {
        List<DeckUnit> eliteDeck = new();
        List<DeckUnit> normalDeck = new();
        _hallDeck = GameManager.Data.GetDeck();

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
        {
            _playerDeck = normalDeck;
        }

        _onSelectAction = onSelectAction;

        _currentPageIndex = 0;
        _maxPageIndex = (_playerDeck.Count - 1) / 10;
        if (_maxPageIndex < 0)
            _maxPageIndex = 0;

        ClearCard();
        SetCard();
        SetPageAllUI();
    }

    public void HallEliteDeckInit(bool isBoss = false, Action<DeckUnit> onSelectAction = null)
    {
        List<DeckUnit> eliteDeck = new();

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to bring to the Divine Hall.");

        _hallDeck = GameManager.Data.GetDeck();

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
        _onSelectAction = onSelectAction;

        _currentPageIndex = 0;
        _maxPageIndex = (_playerDeck.Count - 1) / 10;
        if (_maxPageIndex < 0)
            _maxPageIndex = 0;

        ClearCard();
        SetCard();
        SetPageAllUI();
    }

    private void SetCard(CurrentEvent currentEvent = CurrentEvent.None)
    {
        ClearCard();

        for (int i = 10 * _currentPageIndex; i < (_currentPageIndex + 1) * 10; i++)
        {
            if (i >= _playerDeck.Count)
                break;

            if (currentEvent == CurrentEvent.Stigmata_Give)
            {
                if (_playerDeck[i].GetStigma(true).Count != 0)
                    AddCard(_playerDeck[i]);
            }
            else if (currentEvent == CurrentEvent.Stigmata_Receive)
            {
                if (_playerDeck[i].Data.Rarity != Rarity.Boss)
                    AddCard(_playerDeck[i]);
            }
            else if (currentEvent == CurrentEvent.Stigmata_Select)
            {
                AddCard(_playerDeck[i]);
                _card_dic[_playerDeck[i]].SetDisable(_playerDeck[i].Data.Rarity == Rarity.Boss);
            }
            else
            {
                AddCard(_playerDeck[i]);
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
        if (_currentEvent == CurrentEvent.Stigmata_Give)
        {
            GameManager.UI.ShowPopup<UI_SystemSelect>().Init("CorfirmGiveStigmata", () =>
            {
                OnUnitSelect(unit);
                GameManager.Data.RemoveDeckUnit(unit);
            });
        }
        else
        {
            OnUnitSelect(unit);
        }
    }

    public void OnUnitSelect(DeckUnit unit)
    {
        UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        unitInfo.SetUnit(unit);

        if (_currentEvent == CurrentEvent.Unit_Restoration_Select)
            unitInfo.Restoration(_onSelectAction, _currentEvent, OnSelectRestorationUnit);
        else
            unitInfo.Init(_onSelectAction, _currentEvent);
    }

    public void OnSelectRestorationUnit(DeckUnit unit)
    {
        _card_dic[unit].SelectCard();
    }

    public void SetButtonClick()
    {
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        _endEvent.Invoke();
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");

        if (_quit_txt.text == "Skip" || _quit_txt.text == "선택 안함")
        {
            if (_isBossClear)
            {
                SceneChanger.SceneChange("MainScene");
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
        if (_maxPageIndex == 0)
            _pageText.SetText("");
        else
            _pageText.SetText($"( {_currentPageIndex + 1} / {_maxPageIndex + 1} )");

        _prePageButton.SetActive(_currentPageIndex != 0);
        _postPageButton.SetActive(_currentPageIndex != _maxPageIndex);
    }

    public void OnPrePageButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        _currentPageIndex--;
        SetCard();
        SetPageAllUI();
    }

    public void OnPostPageButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        _currentPageIndex++;
        SetCard();
        SetPageAllUI();
    }
}
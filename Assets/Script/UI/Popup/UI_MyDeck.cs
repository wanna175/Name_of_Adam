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
    [SerializeField] private UI_SortDropdown _sortButton;

    private List<DeckUnit> _playerDeck = new();
    private List<DeckUnit> _hallDeck = new();
    private Dictionary<DeckUnit, UI_Card> _unitCardDictionary = new();//선택된 유닛

    private List<DeckUnit> _selectedUnitList = new();

    private Action<DeckUnit> _onSelectAction;
    private CurrentEvent _currentEvent = CurrentEvent.None;
    private GameObject _eventMenu = null;
    private Action _endEvent;

    private int _currentPageIndex;
    private int _maxPageIndex;

    public void Init(bool isDeckButtonClick = false)
    {
        _setButton.SetActive(false);
        _sortButton.gameObject.SetActive(false);
        _myDeckButton.SetActive(isDeckButtonClick);//내 덱 보기 버튼으로 진입했을 경우

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Possessed units");

        _playerDeck = GameManager.Data.GetSortedDeck(SortMode.Default);

        _currentPageIndex = 0;
        _maxPageIndex = Mathf.Max((_playerDeck.Count - 1) / 10, 0);

        SetPageAllUI();
        SetCard();
    }

    public void EventInit(Action<DeckUnit> onSelectAction = null, CurrentEvent currentEvent = CurrentEvent.None, GameObject eventMenu = null, Action endEvent = null)
    {
        _currentEvent = currentEvent;
        _onSelectAction = onSelectAction;
        _eventMenu = eventMenu;
        _endEvent = endEvent;

        string titleTextKey = "";

        if (_currentEvent == CurrentEvent.Upgrade_Select)
        {
            titleTextKey = "Select a unit to upgrade.";
        }
        else if (_currentEvent == CurrentEvent.Heal_Faith_Select)
        {
            titleTextKey = "Select a unit to restore faith.";
        }
        else if (_currentEvent == CurrentEvent.Stigmata_Select || _currentEvent == CurrentEvent.Corrupt_Stigmata_Select)
        {
            titleTextKey = "Select a unit to bestow stigmata.";
        }
        else if (_currentEvent == CurrentEvent.Stigmata_Receive)
        {
            titleTextKey = "Select a unit to bestow stigmata.";
            _quitButton.SetActive(false);
        }
        else if (_currentEvent == CurrentEvent.Stigmata_Give)
        {
            titleTextKey = "Select a unit to sacrifice.";
        }
        else if (_currentEvent == CurrentEvent.Revert_Unit_Select)
        {
            titleTextKey = "Select units to revert.";
            _setButton.SetActive(true);
        }
        else if (_currentEvent == CurrentEvent.Hall_Delete)
        {
            titleTextKey = "Possessed units";
            _sortButton.gameObject.SetActive(true);
            _sortButton.Init(this);

            if (!GameManager.OutGameData.Data.IsOnTooltipForDivineHall)
            {
                GameManager.OutGameData.Data.IsOnTooltipForDivineHall = true;
                UI_SystemInfo systemInfo = GameManager.UI.ShowPopup<UI_SystemInfo>();
                systemInfo.Init("TooltipForDivineHall", "");

                GameManager.OutGameData.SaveData();
            }
        }

        _title_txt.text = GameManager.Locale.GetLocalizedEventScene(titleTextKey);

        SetCard(_currentEvent);
    }

    public void RefreshDecks(SortMode sortMode)
    {
        _playerDeck = GameManager.Data.GetSortedDeck(sortMode);
        _currentPageIndex = 0;
        _maxPageIndex = Mathf.Max((_playerDeck.Count - 1) / 10, 0);

        SetCard();
        SetPageAllUI();
    }

    public void HallSaveInit(Action<DeckUnit> onSelectAction = null)
    {
        if (!GameManager.OutGameData.Data.IsOnTooltipForDivineHallInBattle)
        {
            GameManager.OutGameData.Data.IsOnTooltipForDivineHallInBattle = true;
            UI_SystemInfo systemInfo = GameManager.UI.ShowPopup<UI_SystemInfo>();
            systemInfo.Init("TooltipForDivineHallInBattle", "");

            GameManager.OutGameData.SaveData();
        }

        _quit_txt.text = GameManager.Locale.GetLocalizedEventScene("Skip");
        _currentEvent = CurrentEvent.Hall_Select;

        List<DeckUnit> totalDeck = new();
        _hallDeck = GameManager.Data.GameData.FallenUnits;

        _sortButton.gameObject.SetActive(false);
        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to bring to the Divine Hall.");

        foreach (DeckUnit unit in _hallDeck)
            totalDeck.Add(unit);

        _playerDeck = totalDeck;
        _onSelectAction = onSelectAction;

        _currentPageIndex = 0;
        _maxPageIndex = Mathf.Max((_playerDeck.Count - 1) / 10, 0);

        ClearCard();
        SetCard();
        SetPageAllUI();
    }

    public void HallDeckInit(bool isBoss = false, Action<DeckUnit> onSelectAction = null)
    {
        List<DeckUnit> eliteDeck = new();
        List<DeckUnit> normalDeck = new();
        _hallDeck = GameManager.Data.GetSortedDeck(SortMode.Default);

        _sortButton.gameObject.SetActive(false);
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
        _maxPageIndex = Mathf.Max((_playerDeck.Count - 1) / 10, 0);

        ClearCard();
        SetCard();
        SetPageAllUI();
    }

    public void HallEliteDeckInit(bool isBoss = false, Action<DeckUnit> onSelectAction = null)
    {
        List<DeckUnit> eliteDeck = new();

        _sortButton.gameObject.SetActive(false);
        _title_txt.text = GameManager.Locale.GetLocalizedEventScene("Select a unit to bring to the Divine Hall.");

        _hallDeck = GameManager.Data.GetSortedDeck(SortMode.Default);

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
        _maxPageIndex = Mathf.Max((_playerDeck.Count - 1) / 10, 0);

        ClearCard();
        SetCard();
        SetPageAllUI();
    }

    public void SelectCard(DeckUnit unit)
    {
        if (_selectedUnitList.Contains(unit))
        {
            _selectedUnitList.Remove(unit);
            _unitCardDictionary[unit].SetSelectHighlight(false);
        }
        else
        {
            _selectedUnitList.Add(unit);
            _unitCardDictionary[unit].SetSelectHighlight(true);
        }
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
                {
                    AddCard(_playerDeck[i]);
                    if (_playerDeck[i].IsMainDeck)
                    {
                        _unitCardDictionary[_playerDeck[i]].SetDisable(() => GameManager.UI.ShowPopup<UI_SystemInfo>().Init("CantGiveStigmata_MainDeck", string.Empty));
                    }
                }
            }
            else if (currentEvent == CurrentEvent.Stigmata_Receive)
            {
                if (_playerDeck[i].Data.Rarity != Rarity.Boss)
                    AddCard(_playerDeck[i]);
            }
            else if (currentEvent == CurrentEvent.Stigmata_Select ||
                currentEvent == CurrentEvent.Corrupt_Stigmata_Select)
            {
                AddCard(_playerDeck[i]);
                if (_playerDeck[i].Data.Rarity == Rarity.Boss)
                {
                    _unitCardDictionary[_playerDeck[i]].SetDisable(() => GameManager.UI.ShowPopup<UI_SystemInfo>().Init("CantGiveStigmata_Boss", string.Empty));
                }
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
        newCard.SetSelectHighlight(_selectedUnitList.Contains(unit));

        _unitCardDictionary[unit] = newCard;
    }

    public void OnClickCard(DeckUnit unit)
    {
        if (_currentEvent == CurrentEvent.Stigmata_Give)
        {
            GameManager.UI.ShowPopup<UI_SystemSelect>().Init("CorfirmGiveStigmata", () =>
            {
                GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

                UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
                unitInfo.SetUnit(unit);
                unitInfo.Init(_onSelectAction, _currentEvent);
                GameManager.Data.RemoveDeckUnit(unit);
            });
        }
        else
        {
            UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
            unitInfo.SetUnit(unit);
            unitInfo.Init(_onSelectAction, _currentEvent);
        }
    }

    public void SetButtonClick()
    {
        GameManager.Sound.Play("UI/UISFX/UIImportantButtonSFX");
        _endEvent.Invoke();
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        if (_quit_txt.text == "Skip" || _quit_txt.text == "선택 안함")
        {
            SceneChanger.SceneChange("MainScene");
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
            _pageText.SetText($"{_currentPageIndex + 1} / {_maxPageIndex + 1}");

        _prePageButton.SetActive(_currentPageIndex != 0);
        _postPageButton.SetActive(_currentPageIndex != _maxPageIndex);
    }

    public void OnPrePageButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        _currentPageIndex--;
        SetCard();
        SetPageAllUI();
    }

    public void OnPostPageButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        _currentPageIndex++;
        SetCard();
        SetPageAllUI();
    }

    public override bool ESCAction()
    {
        if (_currentEvent == CurrentEvent.None)
        {
            GameManager.UI.ClosePopup();
            GameManager.Sound.Play("UI/UISFX/UICloseSFX");

            return true;
        }
        else if (_currentEvent == CurrentEvent.Heal_Faith_Select ||
            _currentEvent == CurrentEvent.Upgrade_Select ||
            _currentEvent == CurrentEvent.Stigmata_Select ||
            _currentEvent == CurrentEvent.Corrupt_Stigmata_Select ||
            _currentEvent == CurrentEvent.Revert_Unit_Select ||
            _currentEvent == CurrentEvent.Stigmata_Give ||
            _currentEvent == CurrentEvent.Hall_Delete)
        {
            Quit();
            return true;
        }

        return false;
    }
}
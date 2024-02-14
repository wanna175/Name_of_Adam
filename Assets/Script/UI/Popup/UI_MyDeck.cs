using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_MyDeck : UI_Popup
{
    [SerializeField] private GameObject CardPrefabs;
    [SerializeField] private Transform Grid;
    [SerializeField] private GameObject Quit_btn;//�����ư
    [SerializeField] private GameObject Set_btn;//���� ��ư
    [SerializeField] private TextMeshProUGUI _quit_txt;//���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI _title_txt;//���� �ؽ�Ʈ
    private List<DeckUnit> _playerDeck = new();
    private List<DeckUnit> _hallDeck = new();
    private Dictionary<DeckUnit, UI_Card> _card_dic = new();//���õ� ����
    private Action<DeckUnit> _onSelect;
    private Action _endEvent;
    private CUR_EVENT evNum = CUR_EVENT.NONE;
    private bool _isBossClear;

    private GameObject _eventMenu = null;

    [SerializeField] private TMP_Text pageText;
    [SerializeField] private GameObject prePageButton;
    [SerializeField] private GameObject postPageButton;
    private int currentPageIndex;
    private int maxPageIndex;

    public void Init(bool battle=false, Action<DeckUnit> onSelect=null,CUR_EVENT Eventnum = CUR_EVENT.NONE,Action endEvent=null)
    {
        Set_btn.SetActive(false);

        if (Eventnum == CUR_EVENT.RECEIVE_STIGMA)
            Quit_btn.SetActive(false);

        if (battle)
            _playerDeck = BattleManager.Data.PlayerDeck;
        else
            _playerDeck = GameManager.Data.GetDeck();

        currentPageIndex = 0;
        maxPageIndex = (_playerDeck.Count - 1) / 10;
        if (maxPageIndex < 0)
            maxPageIndex = 0;

        ClearCard();
        SetPageAllUI();

        if (onSelect != null)
            _onSelect = onSelect;
        if (endEvent != null)
            _endEvent = endEvent;

        IsEventScene(Eventnum);
        evNum = Eventnum;
        
        if (evNum == CUR_EVENT.GIVE_STIGMA)
            SetCard(evNum);
        else
            SetCard();
    }

    public void HallSaveInit(bool isBossClear, Action<DeckUnit> onSelect = null)
    {
        _quit_txt.text = "���� ����";
        _isBossClear = isBossClear;

        List<DeckUnit> normalDeck = new();
        List<DeckUnit> totalDeck = new();
        _hallDeck = GameManager.Data.GameData.FallenUnits;

        currentPageIndex = 0;
        maxPageIndex = (_hallDeck.Count - 1) / 10;
        if (maxPageIndex < 0)
            maxPageIndex = 0;

        _title_txt.text = "���翡 ������ ������ �����ϼ���";

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

    public void HallDeckInit(bool isElite = false, Action<DeckUnit> onSelect = null)
    {
        List<DeckUnit> eliteDeck = new();
        List<DeckUnit> normalDeck = new();

        _hallDeck = GameManager.Data.GetDeck();

        currentPageIndex = 0;
        maxPageIndex = (_hallDeck.Count - 1) / 10;
        if (maxPageIndex < 0)
            maxPageIndex = 0;

        ClearCard();
        SetPageAllUI();

        _title_txt.text = "���翡 ������ ������ �����ϼ���";

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

        if (isElite)
        {
            _playerDeck = eliteDeck;
        }
        else
            _playerDeck = normalDeck;

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }

    public void HallFullDeckInit(Action<DeckUnit> onSelect = null)
    {
        _title_txt.text = "���翡 ������ ������ �����ϼ���";

        _playerDeck = GameManager.Data.GetDeck();

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }

    public void SetCard() 
    {
        var dumpCards = Grid.GetComponentsInChildren<UI_Card>();
        foreach (var card in dumpCards)
            GameManager.Resource.Destroy(card.gameObject);

        for (int i = 10 * currentPageIndex; i < (currentPageIndex + 1) * 10; i++)
        {
            if (i >= _playerDeck.Count)
                break;

            AddCard(_playerDeck[i]);
        }
    }
    private void ClearCard()
    {
        var dumpCards = Grid.GetComponentsInChildren<UI_Card>();
        foreach (var card in dumpCards)
            GameManager.Resource.Destroy(card.gameObject);
    }

    private void SetCard(CUR_EVENT EventNum)
    {
        ClearCard();

        for (int i = 10 * currentPageIndex; i < (currentPageIndex + 1) * 10; i++)
        {
            if (i >= _playerDeck.Count)
                break;

            if (_playerDeck[i].GetStigma(true).Count != 0)
                AddCard(_playerDeck[i]);
        }
    }
    public void AddCard(DeckUnit unit)
    {
        UI_Card newCard = GameObject.Instantiate(CardPrefabs, Grid).GetComponent<UI_Card>();
        newCard.SetCardInfo(this, unit);

        _card_dic[unit] = newCard;
    }

    public void OnClickCard(DeckUnit unit)
    {
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");
        ui.SetUnit(unit);

        if (evNum == CUR_EVENT.HARLOT_RESTORATION)
            ui.Restoration(_onSelect, evNum, OnSelectRestorationUnit);
        else
            ui.Init(_onSelect, evNum);
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

    private void IsEventScene(CUR_EVENT EventScene)
    {
        string sceneName = currentSceneName();
        if (sceneName.Equals("EventScene"))
        {
            //Quit_btn.SetActive(false);

            if (EventScene == CUR_EVENT.UPGRADE)
                _title_txt.text = "��ȭ�� ������ �����ϼ���";
            else if (EventScene == CUR_EVENT.RELEASE)
                _title_txt.text = "�ž��� ȸ����ų ������ �����ϼ���";
            else if (EventScene == CUR_EVENT.STIGMA || EventScene == CUR_EVENT.RECEIVE_STIGMA)
                _title_txt.text = "������ �ο��� ������ �����ϼ���";
            else if (EventScene == CUR_EVENT.GIVE_STIGMA)
                _title_txt.text = "�����ų ������ �����ϼ���";
            else if (EventScene == CUR_EVENT.HARLOT_RESTORATION)
            {
                _title_txt.text = "ȯ����ų ���ֵ��� �����ϼ���";
                Set_btn.SetActive(true);
            }
        }
        else
        {
            _title_txt.text = "���� ����";
        }
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");

        if (_quit_txt.text == "���� ����")
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
        if (maxPageIndex == 0)
            pageText.SetText("");
        else
            pageText.SetText($"( {currentPageIndex + 1} / {maxPageIndex + 1} )");
    }

    private void SetPreButtonUI()
    {
        if (currentPageIndex == 0)
            prePageButton.SetActive(false);
        else
            prePageButton.SetActive(true);
    }

    private void SetPostButtonUI()
    {
        if (currentPageIndex == maxPageIndex)
            postPageButton.SetActive(false);
        else
            postPageButton.SetActive(true);
    }

    public void OnPrePageButton()
    {
        currentPageIndex--;
        SetCard();
        SetPageAllUI();
    }

    public void OnPostPageButton()
    {
        currentPageIndex++;
        SetCard();
        SetPageAllUI();
    }
}

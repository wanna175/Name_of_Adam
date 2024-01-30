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
    private Dictionary<DeckUnit,UI_Card> _card_dic = new();//���õ� ����
    private Action<DeckUnit> _onSelect;
    private Action _endEvent;
    private CUR_EVENT evNum = CUR_EVENT.NONE;
    private bool _isBossClear;

    private GameObject eventMenu = null;

    public void Init(bool battle=false, Action<DeckUnit> onSelect=null,CUR_EVENT Eventnum = CUR_EVENT.NONE,Action endEvent=null)
    {
        Set_btn.SetActive(false);
        if (Eventnum == CUR_EVENT.RECEIVE_STIGMA)
            Quit_btn.SetActive(false);
        if (battle)
            _playerDeck = BattleManager.Data.PlayerDeck;
        else
            _playerDeck = GameManager.Data.GetDeck();

        if (onSelect != null)
            _onSelect = onSelect;
        if (endEvent != null)
            _endEvent = endEvent;
        isEventScene(Eventnum);
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

        List<DeckUnit> _normalDeck = new();
        List<DeckUnit> _totalDeck = new();
        _hallDeck = GameManager.Data.GameData.FallenUnits;
        _title_txt.text = "���翡 ������ ������ �����ϼ���";

        foreach (DeckUnit unit in _hallDeck)
        {
            _totalDeck.Add(unit);

            if (unit.Data.Rarity == Rarity.Normal)
            {
                _normalDeck.Add(unit);
            }
        }

        if (isBossClear)
        {
            _playerDeck = _totalDeck;
        }
        else
        {
            _playerDeck = _normalDeck;
        }
        if (onSelect != null)
            _onSelect = onSelect;
        SetCard();
    }

    public void HallDeckInit(bool isElite = false, Action<DeckUnit> onSelect = null)
    {
        List<DeckUnit> _eliteDeck = new();
        List<DeckUnit> _normalDeck = new();

        _hallDeck = GameManager.Data.GetDeck();

        _title_txt.text = "���翡 ������ ������ �����ϼ���";

        foreach (DeckUnit unit in _hallDeck)
        {
            if (unit.Data.Rarity == Rarity.Normal)
            {
                _normalDeck.Add(unit);
            }
            else
            {
                _eliteDeck.Add(unit);
            }
        }

        if (isElite)
        {
            _playerDeck = _eliteDeck;
        }
        else
            _playerDeck = _normalDeck;

        if (onSelect != null)
            _onSelect = onSelect;

        SetCard();
    }
    public void SetCard() 
    {
        foreach (DeckUnit unit in _playerDeck)
        {
            AddCard(unit);
        }
        
    }
    private void SetCard(CUR_EVENT EventNum)
    {
        foreach (DeckUnit unit in _playerDeck)
        {
            if (unit.GetStigma(true).Count != 0)
            {
                AddCard(unit);
            }
        }
    }
    public void AddCard(DeckUnit unit)
    {
        UI_Card newCard = GameObject.Instantiate(CardPrefabs, Grid).GetComponent<UI_Card>();
        newCard.SetCardInfo(this, unit);

        if(evNum == CUR_EVENT.UPGRADE)
        {
            newCard.SetDisableUpgrade(unit);
        }

        _card_dic[unit] = newCard;
    }

    public void OnClickCard(DeckUnit unit)
    {
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(unit);
        if (evNum == CUR_EVENT.HARLOT_RESTORATION)
            ui.Restoration(_onSelect,evNum, OnSelectRestorationUnit);
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
        this.eventMenu = obj;
    }
    private void isEventScene(CUR_EVENT EventScene)
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
            if (eventMenu != null)
                eventMenu.SetActive(true);
            GameManager.UI.ClosePopup();

        }
    }
}

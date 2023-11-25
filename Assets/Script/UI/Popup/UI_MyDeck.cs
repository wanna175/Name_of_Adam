using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_MyDeck : UI_Popup
{
    [SerializeField] private GameObject CardPrefabs;
    [SerializeField] private Transform Grid;
    [SerializeField] private GameObject Quit_btn;//종료버튼
    [SerializeField] private TextMeshProUGUI _title_txt;//제목 텍스트
    private List<DeckUnit> _playerDeck = new();
    private List<DeckUnit> _hallDeck = new();
    private Action<DeckUnit> _onSelect;
    private int evNum=0;

    public void Init(bool battle=false, Action<DeckUnit> onSelect=null,int Eventnum = 0)
    {
        if (battle)
            _playerDeck = BattleManager.Data.PlayerDeck;
        else
            _playerDeck = GameManager.Data.GetDeck();

        if (onSelect != null)
            _onSelect = onSelect;
        isEventScene(Eventnum);
        evNum = Eventnum;
        if (Eventnum == (int)CUR_EVENT.GIVE_STIGMA)
            SetCard(evNum);
        else
            SetCard();
        
    }
    public void HallSaveInit(bool isBossClear, Action<DeckUnit> onSelect = null)
    {
        List<DeckUnit> _normalDeck = new();

        _hallDeck = GameManager.Data.GameData.FallenUnits;

        foreach (DeckUnit unit in _hallDeck)
        {
            if (unit.Data.Rarity == Rarity.일반)
            {
                _normalDeck.Add(unit);
            }
        }

        if (isBossClear)
        {
            _playerDeck = _hallDeck;
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

        foreach (DeckUnit unit in _hallDeck)
        {
            if (unit.Data.Rarity == Rarity.일반)
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
    public void SetCard()//낙인을 옮길 수 있는 사람만 셋팅하는 용도 
    {
        foreach (DeckUnit unit in _playerDeck)
        {
            AddCard(unit);
        }
        
    }
    private void SetCard(int EventNum)
    {
        foreach (DeckUnit unit in _playerDeck)
        {
            if (unit.GetStigma(EventNum).Count != 0)
            {
                AddCard(unit);
            }
        }
    }
    public void AddCard(DeckUnit unit)
    {
        UI_Card newCard = GameObject.Instantiate(CardPrefabs, Grid).GetComponent<UI_Card>();
        newCard.SetCardInfo(this, unit);
    }

    public void OnClickCard(DeckUnit unit)
    {
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(unit);
        ui.Init(_onSelect,evNum);
    }
    private void isEventScene(int EventScene)
    {
        string sceneName = currentSceneName();
        if (sceneName.Equals("EventScene"))
        {
            Quit_btn.SetActive(false);
            _title_txt.gameObject.SetActive(true);
            //임시로 걍 여기서 부제목 쓰자잉
            if (EventScene == (int)CUR_EVENT.UPGRADE)
                _title_txt.text = "강화할 유닛을 골라바잉~";
            else if (EventScene == (int)CUR_EVENT.RELEASE)
                _title_txt.text = "교화를 풀 유닛을 골라바용";
            else if (EventScene == (int)CUR_EVENT.STIGMA||EventScene == (int)CUR_EVENT.RECEIVE_STIGMA)
                _title_txt.text = "좋은 말로 할때 낙인부여 유닛 골라라";
            else if (EventScene == (int)CUR_EVENT.GIVE_STIGMA)
                _title_txt.text = "희생시킬 놈을 선택해라";
        }
    }
    public void Quit()
    {
        //GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        GameManager.UI.ClosePopup();
    }
}

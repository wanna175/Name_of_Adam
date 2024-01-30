using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UI_UnitInfo : UI_Popup
{
    [SerializeField] private GameObject _selectButton;
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private GameObject _eventButton;
    [SerializeField] private GameObject _completeButton;
    [SerializeField] private GameObject _fallGaugePrefab;
    [SerializeField] private GameObject _stigmaPrefab;
    [SerializeField] private GameObject _upgradeCountPrefab;
    [SerializeField] private GameObject _squarePrefab;

    [SerializeField] private TextMeshProUGUI _unitInfoName;
    [SerializeField] private Transform _unitInfoFallGrid;
    [SerializeField] private TextMeshProUGUI _unitInfoStat;
    [SerializeField] private Transform _unitInfoStigmaGrid;
    [SerializeField] private Transform _unitInfoUpgradeCountGrid;
    [SerializeField] private GameObject _AddedUpgradeCountSocket;
    [SerializeField] private Transform _unitInfoSkillRangeGrid;
    [SerializeField] private Image _unitImage;

    //public AnimationClip _unitAnimationClip;

    private DeckUnit _unit;
    UnitDataSO Data => _unit.Data;
    private Action<DeckUnit> _onSelect;
    private Action _endEvent;
    private Action<DeckUnit> _selectRestorationUnit;
    private CUR_EVENT evNum = 0;
    public void SetUnit(DeckUnit unit)
    {
        _unit = unit;
    }

    public void Init(Action<DeckUnit> onSelect=null,CUR_EVENT Eventnum=CUR_EVENT.NONE,Action endEvent=null)
    {
        _unitImage.GetComponent<Image>();
        _unitImage.sprite = _unit.Data.CorruptImage;
        _onSelect = onSelect;
        _endEvent = endEvent;
        _selectButton.SetActive(onSelect != null);
        evNum = Eventnum;
        if (evNum == CUR_EVENT.COMPLETE_UPGRADE|| evNum == CUR_EVENT.COMPLETE_RELEASE
            ||evNum == CUR_EVENT.COMPLETE_STIGMA ||evNum == CUR_EVENT.COMPLETE_HAELOT)
        {
            _quitButton.SetActive(false);
            _completeButton.SetActive(true);
        }
        if (evNum == CUR_EVENT.STIGMA_EXCEPTION)
            Select();
        _unitInfoName.text = _unit.Data.Name;

        if (_unit.Data.DarkEssenseCost > 0)
        {
            _unitInfoStat.text = "HP: " + _unit.DeckUnitTotalStat.MaxHP.ToString() + "\n" +
                                        "Cost: " + _unit.DeckUnitTotalStat.ManaCost.ToString() + "/" + _unit.Data.DarkEssenseCost.ToString() + "\n" +
                                        "Attack: " + _unit.DeckUnitTotalStat.ATK.ToString() + "\n" +
                                        "Speed: " + _unit.DeckUnitTotalStat.SPD.ToString();
        }
        else
        {
            _unitInfoStat.text = "HP: " + _unit.DeckUnitTotalStat.MaxHP.ToString() + "\n" +
                                        "Cost: " + _unit.DeckUnitTotalStat.ManaCost.ToString() + "\n" +
                                        "Attack: " + _unit.DeckUnitTotalStat.ATK.ToString() + "\n" +
                                        "Speed: " + _unit.DeckUnitTotalStat.SPD.ToString();
        }

        Debug.Log("current:" + _unit.DeckUnitTotalStat.FallCurrentCount + "max:" + _unit.DeckUnitTotalStat.FallMaxCount);
        for (int i = _unit.DeckUnitTotalStat.FallCurrentCount; i < _unit.DeckUnitTotalStat.FallMaxCount; i++)
        {
            UI_FallUnit fu = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallUnit>();
            //UI_FallGauge fg = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallGauge>();
            fu.SwitchCountImage(Team.Player);
            fu.EmptyGauge();

            //fg.Init();
        }

        List<Stigma> stigmas = _unit.GetStigma();
        for (int i = 0; i < _unit._maxStigmaCount; i++)
        {
            UI_HoverImageBlock ui = GameObject.Instantiate(_stigmaPrefab, _unitInfoStigmaGrid).GetComponent<UI_HoverImageBlock>();
            if (i < stigmas.Count)
            {
                ui.Set(stigmas[i].Sprite_88, stigmas[i].Description);
                ui.EnableUI(true);
            }
            else
                ui.EnableUI(false);
        }

        for (int i = 0; i < _unit.DeckUnitTotalStat.CurrentUpgradeCount; i++)
        {
            GameObject.Instantiate(_upgradeCountPrefab, _unitInfoUpgradeCountGrid);
        }

        foreach (bool range in _unit.Data.AttackRange)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _unitInfoSkillRangeGrid).GetComponent<Image>();
            if (range)
                block.color = Color.red;
            else
                block.color = Color.grey;
        }

        if (GameManager.OutGameData.IsUnlockedItem(12))
        {
            _AddedUpgradeCountSocket.SetActive(true);
        }
    }
    public void Restoration(Action<DeckUnit> OnSelect=null, CUR_EVENT Eventnum = CUR_EVENT.NONE,Action<DeckUnit> selectRestorationUnit=null)
    {
        this.Init(OnSelect,Eventnum);
        _selectRestorationUnit = selectRestorationUnit;
    } 
    public void SetAnimation()
    {
        AnimationClip clip = Resources.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UnitSpawnBackEffect");
        GameManager.VisualEffect.StartVisualEffect(clip, new Vector3(0f, 3.5f, 0f));
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        GameManager.UI.ClosePopup();
    }

    public void Select()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if(_selectRestorationUnit!=null)
            _selectRestorationUnit(_unit);
        _onSelect(_unit);

        if (currentSceneName().Equals("EventScene")&&evNum!=CUR_EVENT.HARLOT_RESTORATION)
        {
            _quitButton.SetActive(false);
            _selectButton.SetActive(false);
            _eventButton.SetActive(true);
            
            //선택하기 버튼 없애고, 껏다켯다 버튼 만든다.
        }
    }
    public void eventButtonClick()
    {
        switch (evNum)
        {
            case CUR_EVENT.STIGMA_EXCEPTION:
            case CUR_EVENT.UPGRADE:
            case CUR_EVENT.STIGMA://강화하기, 스티그마 부여하기
            case CUR_EVENT.GIVE_STIGMA:
                Transform e = this.transform.parent.GetChild(0);
                e.SetAsLastSibling();
                e.gameObject.SetActive(true);
                break;
            case CUR_EVENT.RECEIVE_STIGMA:
                break;
            default:
                break;
        }
    }
    public void compeleteClick()
    {
        if (evNum == CUR_EVENT.COMPLETE_HAELOT && _onSelect != null)
            _onSelect(_unit);
        gameObject.SetActive(false);
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (_endEvent != null)
            _endEvent.Invoke();
    }
}
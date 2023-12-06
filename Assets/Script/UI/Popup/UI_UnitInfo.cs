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
    [SerializeField] private GameObject _squarePrefab;

    [SerializeField] private TextMeshProUGUI _unitInfoName;
    [SerializeField] private Transform _unitInfoFallGrid;
    [SerializeField] private TextMeshProUGUI _unitInfoStat;
    [SerializeField] private Transform _unitInfoStigmaGrid;
    [SerializeField] private Transform _unitInfoSkillRangeGrid;
    [SerializeField] private Image _unitImage;

    //public AnimationClip _unitAnimationClip;

    private DeckUnit _unit;
    UnitDataSO Data => _unit.Data;
    private Action<DeckUnit> _onSelect;
    private Action _endEvent;
    private int evNum = 0;
    public void SetUnit(DeckUnit unit)
    {
        _unit = unit;
    }

    public void Init(Action<DeckUnit> onSelect=null,int Eventnum=0,Action endEvent=null)
    {
        _unitImage.GetComponent<Image>();
        _unitImage.sprite = _unit.Data.Image;
        _onSelect = onSelect;
        _endEvent = endEvent;
        _selectButton.SetActive(onSelect != null);
        evNum = Eventnum;
        if (evNum == (int)CUR_EVENT.COMPLETE_UPGRADE|| evNum == (int)CUR_EVENT.COMPLETE_RELEASE
            ||evNum == (int)CUR_EVENT.COMPLETE_STIGMA)
        {
            _quitButton.SetActive(false);
            _completeButton.SetActive(true);
        }
        if (evNum == (int)CUR_EVENT.STIGMA_EXCEPTION)
            Select();
        _unitInfoName.text = _unit.Data.Name;

        _unitInfoStat.text =    "HP:\t" + _unit.DeckUnitTotalStat.MaxHP.ToString() + "\n" +
                                    "Cost:\t" + _unit.DeckUnitTotalStat.ManaCost.ToString() + "\n" +
                                    "Attack:\t" + _unit.DeckUnitTotalStat.ATK.ToString() + "\n" +
                                    "Speed:\t" + _unit.DeckUnitTotalStat.SPD.ToString();

        for (int i = 0; i < _unit.DeckUnitTotalStat.FallMaxCount; i++)
        {
            UI_FallUnit fu = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallUnit>();
            //UI_FallGauge fg = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallGauge>();
            fu.SwitchCountImage(Team.Player);
            if (i < _unit.DeckUnitTotalStat.FallCurrentCount)
                fu.FillGauge();
            else
                fu.EmptyGauge();

            //fg.Init();
        }

        foreach (Stigma sti in _unit.GetStigma())
        {
            GameObject.Instantiate(_stigmaPrefab, _unitInfoStigmaGrid).GetComponent<UI_HoverImageBlock>().Set(sti.Sprite, sti.Description);
        }

        foreach (bool range in _unit.Data.AttackRange)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _unitInfoSkillRangeGrid).GetComponent<Image>();
            if (range)
                block.color = Color.red;
            else
                block.color = Color.grey;
        }
    }
    public void SetAnimation()
    {
        AnimationClip clip = Resources.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UnitSpawnBackEffect");
        GameManager.VisualEffect.StartVisualEffect(clip, new Vector3(0f, 3.5f, 0f));
    }

    public void Quit()
    {
        GameManager.UI.ClosePopup();
    }

    public void Select()
    {
        _onSelect(_unit);
        if (currentSceneName().Equals("EventScene"))
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
            case (int)CUR_EVENT.STIGMA_EXCEPTION:
            case (int)CUR_EVENT.UPGRADE:
            case (int)CUR_EVENT.STIGMA://강화하기, 스티그마 부여하기
            case (int)CUR_EVENT.GIVE_STIGMA:
                Transform e = this.transform.parent.GetChild(0);
                e.SetAsLastSibling();
                e.gameObject.SetActive(true);
                break;
            case (int)CUR_EVENT.RECEIVE_STIGMA:
                break;
            default:
                break;
        }
    }
    public void compeleteClick()
    {
        if (_endEvent != null)
            _endEvent.Invoke();
    }
}
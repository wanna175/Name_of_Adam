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
    [SerializeField] private TextMeshProUGUI _quitButtonTxt;
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
    private CurrentEvent _currentEvent = CurrentEvent.None;

    readonly string UpColorStr = "red";
    readonly string DownColorStr = "blue";

    public void SetUnit(DeckUnit unit)
    {
        _unit = unit;
    }

    public void Init(Action<DeckUnit> onSelect = null, CurrentEvent currentEvent = CurrentEvent.None, Action endEvent=null)
    {
        _unitImage.sprite = _unit.Data.CorruptImage;

        _onSelect = onSelect;
        _endEvent = endEvent;

        _selectButton.SetActive(onSelect != null);

        _currentEvent = currentEvent;

        if (_currentEvent == CurrentEvent.Complete_Upgrade || _currentEvent == CurrentEvent.Complete_Heal_Faith
            || _currentEvent == CurrentEvent.Complate_Stigmata || _currentEvent == CurrentEvent.COMPLETE_HAELOT)
        {
            _quitButton.SetActive(false);
            _completeButton.SetActive(true);
        }
        else if (_currentEvent == CurrentEvent.Stigmata_Full_Exception || _currentEvent == CurrentEvent.Upgrade_Full_Exception)
        {
            Select();
        }

        _unitInfoName.text = _unit.Data.Name;

        string darkEssenseCost = (_unit.Data.DarkEssenseCost > 0) ? " / " + _unit.Data.DarkEssenseCost.ToString() : "";

        string hpChange = "";

        if (_unit.DeckUnitStat.MaxHP - _unit.Data.RawStat.MaxHP > 0)
        {
            hpChange = " <color=\"" + UpColorStr + "\">(+" + (_unit.DeckUnitStat.MaxHP - _unit.Data.RawStat.MaxHP).ToString() + ")</color>";
        }
        else if (_unit.DeckUnitStat.MaxHP - _unit.Data.RawStat.MaxHP < 0)
        {
            hpChange = " <color=\"" + DownColorStr + "\">(" + (_unit.DeckUnitStat.MaxHP - _unit.Data.RawStat.MaxHP).ToString() + ")</color>";
        }

        string costChange = "";
        if (_unit.DeckUnitStat.ManaCost - _unit.Data.RawStat.ManaCost > 0)
        {
            costChange = " <color=\"" + DownColorStr + "\">(+" + (_unit.DeckUnitStat.ManaCost - _unit.Data.RawStat.ManaCost).ToString() + ")</color>";
        }
        else if (_unit.DeckUnitStat.ManaCost - _unit.Data.RawStat.ManaCost < 0)
        {
            costChange = " <color=\"" + UpColorStr + "\">(" + (_unit.DeckUnitStat.ManaCost - _unit.Data.RawStat.ManaCost).ToString() + ")</color>";
        }

        string attackChange = "";
        if (_unit.DeckUnitStat.ATK - _unit.Data.RawStat.ATK > 0)
        {
            attackChange = " <color=\"" + UpColorStr + "\">(+" + (_unit.DeckUnitStat.ATK - _unit.Data.RawStat.ATK).ToString() + ")</color>";
        }
        else if (_unit.DeckUnitStat.ATK - _unit.Data.RawStat.ATK < 0)
        {
            attackChange = " <color=\"" + DownColorStr + "\">(" + (_unit.DeckUnitStat.ATK - _unit.Data.RawStat.ATK).ToString() + ")</color>";
        }

        string speedChange = "";
        if (_unit.DeckUnitStat.SPD - _unit.Data.RawStat.SPD > 0)
        {
            speedChange = " <color=\"" + UpColorStr + "\">(+" + (_unit.DeckUnitStat.SPD - _unit.Data.RawStat.SPD).ToString() + ")</color>";
        }
        else if (_unit.DeckUnitStat.SPD - _unit.Data.RawStat.SPD < 0)
        {
            speedChange = " <color=\"" + DownColorStr + "\">(" + (_unit.DeckUnitStat.SPD - _unit.Data.RawStat.SPD).ToString() + ")</color>";
        }

        _unitInfoStat.text = GameManager.Locale.GetLocalizedUpgrade("HP") + ": " + _unit.DeckUnitTotalStat.MaxHP.ToString() + hpChange + "\n" +
                                    GameManager.Locale.GetLocalizedUpgrade("Cost") + ": " + _unit.DeckUnitTotalStat.ManaCost.ToString() + costChange + darkEssenseCost + "\n" +
                                    GameManager.Locale.GetLocalizedUpgrade("Attack") + ": " + _unit.DeckUnitTotalStat.ATK.ToString() + attackChange + "\n" +
                                    GameManager.Locale.GetLocalizedUpgrade("Speed") + ": " + _unit.DeckUnitTotalStat.SPD.ToString() + speedChange;

        for (int i = 0; i < 4; i++)
        {
            UI_FallUnit fu = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallUnit>();
            int fallType = i / 4;

            fu.InitFall(Team.Player, fallType);
            if (i >= _unit.DeckUnitTotalStat.FallMaxCount - _unit.DeckUnitTotalStat.FallCurrentCount)
                fu.SetVisible(false);
        }

        List<Stigma> stigmas = _unit.GetStigma();
        for (int i = 0; i < _unit.MaxStigmaCount; i++)
        {
            UI_HoverImageBlock ui = GameObject.Instantiate(_stigmaPrefab, _unitInfoStigmaGrid).GetComponent<UI_HoverImageBlock>();
            if (i < stigmas.Count)
            {
                ui.Set(stigmas[i].Sprite_88, "<size=150%>" + stigmas[i].Name + "</size>" + "\n\n" + stigmas[i].Description);
                ui.EnableUI(true);
            }
            else
                ui.EnableUI(false);
        }

        int createUpgradeCount = (GameManager.OutGameData.IsUnlockedItem(12)) ? 3 : 2;
        List<Upgrade> upgrades = _unit.DeckUnitUpgrade;
        for (int i = 0; i < createUpgradeCount; i++)
        {
            UI_HoverImageBlock ui = GameObject.Instantiate(_upgradeCountPrefab, _unitInfoUpgradeCountGrid).GetComponent<UI_HoverImageBlock>();
            if (i < upgrades.Count)
            {
                ui.Set(upgrades[i].UpgradeImage88, "<size=150%>" + upgrades[i].UpgradeDescription + "</size>");
                ui.EnableUI(true);
            }
            else
                ui.EnableUI(false);
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

    public void Restoration(Action<DeckUnit> OnSelect=null, CurrentEvent Eventnum = CurrentEvent.None, Action<DeckUnit> selectRestorationUnit=null)
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
        if (currentSceneName().Equals("EventScene") && _currentEvent == CurrentEvent.Heal_Faith_Select)
        {
            GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        }
        else
        {
            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        }

        if (_selectRestorationUnit != null)
        {
            _selectRestorationUnit(_unit);
        }

        if (_onSelect != null)
        {
            switch (_currentEvent)
            {
                case CurrentEvent.Stigmata_Select:
                    if (_unit.GetStigmaCount() == 3)
                    {
                        GameManager.UI.ShowPopup<UI_SystemSelect>().Init("StigmaMax", () =>
                        {
                            _onSelect(_unit);
                            _quitButton.SetActive(false);
                            _selectButton.SetActive(false);
                            _eventButton.SetActive(true);
                        });
                        return;
                    }
                    break;

                case CurrentEvent.Upgrade_Select:
                    if (_unit.DeckUnitUpgrade.Count == 3 || (_unit.DeckUnitUpgrade.Count == 2 && !GameManager.OutGameData.IsUnlockedItem(12)))
                    {
                        GameManager.UI.ShowPopup<UI_SystemSelect>().Init("UpgradeMax", () =>
                        {
                            _onSelect(_unit);
                            _quitButton.SetActive(false);
                            _selectButton.SetActive(false);
                            _eventButton.SetActive(true);
                        });
                        return;
                    }
                    break;
            }

            _onSelect(_unit);
        }

        if (currentSceneName().Equals("EventScene") && _currentEvent != CurrentEvent.Unit_Restoration_Select)
        {
            _quitButton.SetActive(false);
            _selectButton.SetActive(false);
            _eventButton.SetActive(true);
        }
    }

    public void EventButtonClick()
    {
        switch (_currentEvent)
        {
            case CurrentEvent.Stigmata_Full_Exception:
            case CurrentEvent.Upgrade_Select:
            case CurrentEvent.Upgrade_Full_Exception:
            case CurrentEvent.Stigmata_Select://강화하기, 스티그마 부여하기
            case CurrentEvent.Stigmata_Give:
                Transform e = this.transform.parent.GetChild(0);
                e.SetAsLastSibling();
                e.gameObject.SetActive(true);
                break;
            case CurrentEvent.Stigmata_Receive:
                break;
            default:
                break;
        }
    }

    public void CompeleteButtonClick()
    {
        if (_currentEvent == CurrentEvent.COMPLETE_HAELOT && _onSelect != null)
            _onSelect(_unit);
        gameObject.SetActive(false);
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (_endEvent != null)
            _endEvent.Invoke();
    }
}
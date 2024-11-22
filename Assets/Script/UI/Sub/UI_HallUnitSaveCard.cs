using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HallUnitSaveCard : MonoBehaviour
{
    [SerializeField] private GameObject _eliteFrame;
    [SerializeField] private GameObject _normalFrame;

    [SerializeField] private Image unitImage;
    [SerializeField] private TextMeshProUGUI unitName;

    [SerializeField] private TextMeshProUGUI _statHealth;
    [SerializeField] private TextMeshProUGUI _statAttack;
    [SerializeField] private TextMeshProUGUI _statSpeed;
    [SerializeField] private TextMeshProUGUI _statCost;

    [SerializeField] private GameObject _statDarkEssence;
    [SerializeField] private TextMeshProUGUI _statDarkEssenceCost;

    [SerializeField] private TextMeshProUGUI _statHealthChange;
    [SerializeField] private TextMeshProUGUI _statAttackChange;
    [SerializeField] private TextMeshProUGUI _statSpeedChange;
    [SerializeField] private TextMeshProUGUI _statCostChange;

    [SerializeField] private GameObject _stigmaPrefab;
    [SerializeField] private GameObject _upgradeCountPrefab;
    [SerializeField] private Transform _unitInfoStigmaGrid;
    [SerializeField] private Transform _unitInfoUpgradeCountGrid;

    [SerializeField] private Image _insignia;

    [SerializeField] private Image _apostleInsignia;
    [SerializeField] private Image _eliteInsignia;
    [SerializeField] private Image _bossInsignia;

    private DeckUnit _deckUnit;

    readonly Color _upColor = new(1f, 0.22f, 0.22f);
    readonly Color _downColor = new(0.35f, 0.35f, 1f);

    readonly Color _playerInsigniaColor = new(0.35f, 0.09f, 0.05f);
    readonly Color _enemyInsigniaColor = new(0.54f, 0.5f, 0.34f);

    public void Init(DeckUnit deckUnit)
    {
        this._deckUnit = deckUnit;

        _eliteFrame.SetActive(deckUnit.Data.Rarity != Rarity.Normal);
        _normalFrame.SetActive(deckUnit.Data.Rarity == Rarity.Normal);

        unitImage.sprite = deckUnit.Data.CorruptDiaPortraitImage;
        unitName.SetText(deckUnit.Data.Name);

        List<Stigma> stigmas = deckUnit.GetStigma();
        for (int i = 0; i < deckUnit.MaxStigmaCount; i++)
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
        List<Upgrade> upgrades = deckUnit.DeckUnitUpgrade;
        for (int i = 0; i < createUpgradeCount; i++)
        {
            UI_HoverImageBlock ui = GameObject.Instantiate(_upgradeCountPrefab, _unitInfoUpgradeCountGrid).GetComponent<UI_HoverImageBlock>();
            if (i < upgrades.Count)
            {
                ui.Set(upgrades[i].UpgradeImage88, GameManager.Data.UpgradeController.GetUpgradeFullDescription(upgrades[i]));
                ui.EnableUI(true);
            }
            else
                ui.EnableUI(false);
        }

        _statHealth.text = deckUnit.DeckUnitStat.MaxHP.ToString();
        _statAttack.text = deckUnit.DeckUnitStat.ATK.ToString();
        _statSpeed.text = deckUnit.DeckUnitStat.SPD.ToString();
        _statCost.text = deckUnit.DeckUnitStat.ManaCost.ToString();

        _statDarkEssence.SetActive(deckUnit.Data.DarkEssenseCost > 0);
        _statDarkEssenceCost.text = deckUnit.Data.DarkEssenseCost.ToString();

        _statHealthChange.text = (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP > 0) ? "+" : "";
        if (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP != 0)
        {
            _statHealthChange.text += (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP).ToString();
            _statHealthChange.color = (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP > 0) ? _upColor : _downColor;
        }

        _statAttackChange.text = (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK > 0) ? "+" : "";
        if (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK != 0)
        {
            _statAttackChange.text += (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK).ToString();
            _statAttackChange.color = (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK > 0) ? _upColor : _downColor;
        }

        _statSpeedChange.text = (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD > 0) ? "+" : "";
        if (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD != 0)
        {
            _statSpeedChange.text += (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD).ToString();
            _statSpeedChange.color = (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD > 0) ? _upColor : _downColor;
        }

        _statCostChange.text = (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost > 0) ? "+" : "";
        if (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost != 0)
        {
            _statCostChange.text += (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost).ToString();
            _statCostChange.color = (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost > 0) ? _downColor : _upColor;
        }

        _insignia.color =  _playerInsigniaColor;

        _apostleInsignia.gameObject.SetActive(deckUnit.Data.Rarity == Rarity.Original);
        _eliteInsignia.gameObject.SetActive(deckUnit.Data.Rarity == Rarity.Elite);
        _bossInsignia.gameObject.SetActive(deckUnit.Data.Rarity == Rarity.Boss);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HallUnitSaveCard : MonoBehaviour
{
    readonly string UpColorStr = "red";
    readonly string DownColorStr = "blue";

    [SerializeField] private GameObject _eliteFrame;
    [SerializeField] private GameObject _normalFrame;

    [SerializeField] private Image unitImage;
    [SerializeField] private TextMeshProUGUI unitName;

    [SerializeField] private TextMeshProUGUI _statInfo;
    [SerializeField] private TextMeshProUGUI _statNumber;

    [SerializeField] private GameObject _stigmaPrefab;
    [SerializeField] private GameObject _upgradeCountPrefab;
    [SerializeField] private Transform _unitInfoStigmaGrid;
    [SerializeField] private Transform _unitInfoUpgradeCountGrid;

    private DeckUnit _deckUnit;

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

        List<Upgrade> upgrades = deckUnit.DeckUnitUpgrade;
        for (int i = 0; i < deckUnit.MaxUpgradeCount; i++)
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

        _statInfo.text = $"{GameManager.Locale.GetLocalizedUpgrade("HP")}\n" +
            $"{GameManager.Locale.GetLocalizedUpgrade("Cost")}\n" +
            $"{GameManager.Locale.GetLocalizedUpgrade("Attack")}\n" +
            $"{GameManager.Locale.GetLocalizedUpgrade("Speed")}\n";

        string hpChange = "";
        if (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP > 0)
            hpChange = " <color=\"" + UpColorStr + "\">(+" + (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP).ToString() + ")</color>";
        else if (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP < 0)
            hpChange = " <color=\"" + DownColorStr + "\">(" + (deckUnit.DeckUnitStat.MaxHP - deckUnit.Data.RawStat.MaxHP).ToString() + ")</color>";

        string costChange = "";
        if (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost > 0)
            costChange = " <color=\"" + DownColorStr + "\">(+" + (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost).ToString() + ")</color>";
        else if (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost < 0)
            costChange = " <color=\"" + UpColorStr + "\">(" + (deckUnit.DeckUnitStat.ManaCost - deckUnit.Data.RawStat.ManaCost).ToString() + ")</color>";

        string attackChange = "";
        if (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK > 0)
            attackChange = " <color=\"" + UpColorStr + "\">(+" + (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK).ToString() + ")</color>";
        else if (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK < 0)
            attackChange = " <color=\"" + DownColorStr + "\">(" + (deckUnit.DeckUnitStat.ATK - deckUnit.Data.RawStat.ATK).ToString() + ")</color>";

        string speedChange = "";
        if (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD > 0)
            speedChange = " <color=\"" + UpColorStr + "\">(+" + (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD).ToString() + ")</color>";
        else if (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD < 0)
            speedChange = " <color=\"" + DownColorStr + "\">(" + (deckUnit.DeckUnitStat.SPD - deckUnit.Data.RawStat.SPD).ToString() + ")</color>";

        string darkEssenseCost = (deckUnit.Data.DarkEssenseCost > 0) ? " / " + deckUnit.Data.DarkEssenseCost.ToString() : "";

        _statNumber.text = deckUnit.DeckUnitTotalStat.MaxHP.ToString() + hpChange + "\n" +
                           deckUnit.DeckUnitTotalStat.ManaCost.ToString() + costChange + darkEssenseCost + "\n" +
                           deckUnit.DeckUnitTotalStat.ATK.ToString() + attackChange + "\n" +
                           deckUnit.DeckUnitTotalStat.SPD.ToString() + speedChange;
    }
}

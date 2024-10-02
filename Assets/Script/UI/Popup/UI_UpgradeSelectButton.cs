using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_UpgradeSelectButton : UI_Base
{
    [SerializeReference] private Image _upgradeImage;
    [SerializeReference] private TextMeshProUGUI _upgradeName;
    [SerializeReference] private TextMeshProUGUI _upgradeDescription;
    [SerializeReference] private GameObject _goldFrame;
    [SerializeReference] private Button _button;

    [SerializeReference] private GameObject _hp;
    [SerializeReference] private GameObject _atk;
    [SerializeReference] private GameObject _spd;
    [SerializeReference] private GameObject _cost;

    [SerializeReference] private TextMeshProUGUI _hpText;
    [SerializeReference] private TextMeshProUGUI _atkText;
    [SerializeReference] private TextMeshProUGUI _spdText;
    [SerializeReference] private TextMeshProUGUI _costText;

    readonly Color _goldTextColor = new(0.82f, 0.65f, 0.27f);
    readonly Color _orangeTextColor = new(1f, 0.55f, 0f);

    readonly Color _downTextColor = new(0.5f, 0.5f, 1f);

    private UI_UpgradeSelectButtonPopup _popup;
    private Upgrade _upgrade;
    private int _selectIndex;

    public void Init(Upgrade upgrade, int selectIndex, UI_UpgradeSelectButtonPopup popup)
    {
        _popup = popup;
        _selectIndex = selectIndex;
        _upgrade = upgrade;
        _button.onClick.AddListener(OnClick);

        _upgradeImage.sprite = upgrade.UpgradeImage160;

        if (upgrade.UpgradeData.Rarity == 2)
            _upgradeName.color = _goldTextColor;
        else if (upgrade.UpgradeData.Rarity == 3)
            _upgradeName.color = _orangeTextColor;

        _upgradeName.text = GameManager.Locale.GetLocalizedUpgrade(upgrade.UpgradeName);

        _upgradeDescription.SetText(GameManager.Data.UpgradeController.GetUpgradeDescription(upgrade));
        _goldFrame.SetActive(upgrade.UpgradeData.Rarity > 1);

        if (upgrade.UpgradeStat.CurrentHP != 0)
        {
            _hp.SetActive(true);
            if (upgrade.UpgradeStat.CurrentHP < 0)
            {
                _hpText.color = _downTextColor;
                _hp.transform.SetAsLastSibling();
            }

            _hpText.text = upgrade.UpgradeStat.CurrentHP.ToString();
        }
        if (upgrade.UpgradeStat.ATK != 0)
        {
            _atk.SetActive(true);
            if (upgrade.UpgradeStat.ATK < 0)
            {
                _atkText.color = _downTextColor;
                _atk.transform.SetAsLastSibling();
            }
            _atkText.text = upgrade.UpgradeStat.ATK.ToString();
        }
        if (upgrade.UpgradeStat.SPD != 0)
        {
            _spd.SetActive(true);
            if (upgrade.UpgradeStat.SPD < 0)
            {
                _spdText.color = _downTextColor;
                _spd.transform.SetAsLastSibling();
            }
            _spdText.text = upgrade.UpgradeStat.SPD.ToString();
        }
        if (upgrade.UpgradeStat.ManaCost != 0)
        {
            _cost.SetActive(true);
            if (upgrade.UpgradeStat.ManaCost > 0)
            {
                _costText.color = _downTextColor;
                _cost.transform.SetAsLastSibling();
            }
            _costText.text = upgrade.UpgradeStat.ManaCost.ToString();
        }
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        _popup.OnClickUpgradeButton(_selectIndex);
    }
}

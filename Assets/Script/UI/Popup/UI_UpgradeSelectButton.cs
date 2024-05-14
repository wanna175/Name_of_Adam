using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_UpgradeSelectButton : UI_Base
{
    [SerializeReference] private Image _upgradeImage;
    [SerializeReference] private TextMeshProUGUI _upgradeDescription;
    [SerializeReference] private GameObject _goldFrame;
    [SerializeReference] private Button _button;

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
        _upgradeDescription.SetText(upgrade.UpgradeDescription);
        _goldFrame.SetActive(upgrade.UpgradeData.Rarity > 1);
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        _popup.OnClickUpgradeButton(_selectIndex);
    }
}

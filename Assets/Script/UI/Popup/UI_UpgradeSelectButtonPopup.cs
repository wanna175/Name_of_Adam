using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_UpgradeSelectButtonPopup : UI_Popup
{
    [SerializeField] private UI_UpgradeSelectButton _buttonPrefab;
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _rerollButton;

    private UpgradeSceneController _uc;
    private bool _isUpgradeFull;

    public void Init(UpgradeSceneController uc, List<Upgrade> upgrades, bool isUpgradeFull)
    {
        _uc = uc;
        _isUpgradeFull = isUpgradeFull;

        if (_isUpgradeFull)
        {
            _titleText.SetText(GameManager.Locale.GetLocalizedEventScene("Full Upgrade"));
        }
        else
        {
            _titleText.SetText(GameManager.Locale.GetLocalizedEventScene("Select Upgrade"));
        }

        _rerollButton.SetActive(false);
        if (!isUpgradeFull)
            _rerollButton.SetActive(GameManager.OutGameData.GetProgressSave(22).isUnLock);

        int createButtonCount = upgrades.Count;

        if (createButtonCount == 4)
            _grid.spacing = new Vector2(150, 10);
        else
            _grid.spacing = new Vector2(200, 10);

        for (int i = 0; i < createButtonCount; i++)
        {
            GameObject.Instantiate(_buttonPrefab, _grid.transform).GetComponent<UI_UpgradeSelectButton>().Init(upgrades[i], i, this);
        }
    }

    public void ResetUpgradeSelectButtons()
    {
        var buttons = _grid.GetComponentsInChildren<UI_UpgradeSelectButton>();
        foreach (var button in buttons)
            Destroy(button.gameObject);

        var upgradeList = _uc.ResetUpgrade();
        Init(_uc, upgradeList, _isUpgradeFull);
    }

    public void OnClickUpgradeButton(int select)
    {
        if (_isUpgradeFull)
        {
            _uc.OnDestroyUpgradeSelect(select);
        }
        else
        {
            _uc.OnUpgradeSelect(select);
        }
    }

    public void QuitBtn()
    {
        this.transform.SetAsFirstSibling();
        this.gameObject.SetActive(false);
    }

    public void Reroll()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        ResetUpgradeSelectButtons();
        _rerollButton.SetActive(false);
    }
}

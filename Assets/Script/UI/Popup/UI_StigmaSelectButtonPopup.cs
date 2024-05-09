using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StigmaSelectButtonPopup : UI_Popup
{
    [SerializeField] private UI_StigmaSelectButton _buttonPrefab;
    [SerializeField] private Transform _grid;
    [SerializeField] private TextMeshProUGUI _titleText;

    private DeckUnit _targetUnit;
    private int _stigmataCount;

    private Action _afterPopupAction;
    private StigmaInterface _stigmataController;
    private CurrentEvent _currentEvent;

    private bool _isStigmataFull;
    private bool _isCanReset;

    public void Init(DeckUnit targetUnit, bool isStigmataFull, List<Stigma> stigmataSelectList, Action afterPopupAction = null)
    {
        _targetUnit = targetUnit;//¹ÞÀ» À¯´Ö

        _isStigmataFull = isStigmataFull;
        _isCanReset = !isStigmataFull;

        _afterPopupAction = afterPopupAction;

        _titleText.SetText(GameManager.Locale.GetLocalizedEventScene("Select Stigma"));
        SetStigmaSelectButtons(stigmataSelectList);
    }

    public void EventInit(StigmaInterface stigmataController, CurrentEvent currentEvent = CurrentEvent.None)
    {
        _stigmataController = stigmataController;
        _currentEvent = currentEvent;
        if (currentEvent == CurrentEvent.Stigmata_Full_Exception)
        {
            _titleText.SetText(GameManager.Locale.GetLocalizedEventScene("Full Stigma"));
        }
        else if (currentEvent == CurrentEvent.Stigmata_Give)
        {
            _titleText.SetText(GameManager.Locale.GetLocalizedEventScene("Transfer Stigma Give"));
            _isCanReset = false;
        }
    }

    public void ResetStigmataSelectButtons()
    {
        if (!_isCanReset)
            return;

        List<Stigma> stigmaList = _stigmataController.ResetStigmataList(_targetUnit);
        
        var buttons = _grid.GetComponentsInChildren<UI_StigmaSelectButton>();
        foreach (var button in buttons)
            Destroy(button.gameObject);

        SetStigmaSelectButtons(stigmaList);
    }

    private void SetStigmaSelectButtons(List<Stigma> stigmataList)
    {
        foreach (Stigma stigmata in stigmataList)
        {
            GameObject.Instantiate(_buttonPrefab, _grid).GetComponent<UI_StigmaSelectButton>().Init(stigmata, this);
        }
    }

    public void OnClickStigmataButton(Stigma stigmata)
    {
        if (_stigmataController != null)
        {
            _stigmataController.OnStigmataSelected(stigmata);
        }
        else
        {
            if (TutorialManager.Instance.IsEnable())
                TutorialManager.Instance.ShowNextTutorial();

            if (SceneChanger.GetSceneName() == "BattleScene")
            {
                GameObject.Find("@UI_Root").transform.Find("UI_StigmaSelectBlocker").gameObject.SetActive(false);
                BattleManager.Data.CorruptionPopups.RemoveAt(BattleManager.Data.CorruptionPopups.Count - 1);
                BattleManager.Data.IsCorruptionPopupOn = false;
            }

            if (_targetUnit != null)
                _targetUnit.AddStigma(stigmata);

            GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
            if (_afterPopupAction != null)
            {
                _afterPopupAction.Invoke();
            }

            GameManager.UI.ClosePopup();
        }
    }

    public void QuitBtn()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (SceneChanger.GetSceneName() == "BattleScene")
        {
            GameObject.Find("@UI_Root").transform.Find("UI_StigmaSelectBlocker").gameObject.SetActive(true);
        }
        this.transform.SetAsFirstSibling();
        this.gameObject.SetActive(false);
    }
}
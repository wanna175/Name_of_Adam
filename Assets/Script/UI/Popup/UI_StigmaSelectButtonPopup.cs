using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StigmaSelectButtonPopup : UI_Popup
{
    private Action _afterPopupAction;
    private DeckUnit _targetUnit;
    [SerializeField] private UI_StigmaSelectButton _buttonPrefab;
    [SerializeField] private Transform _grid;
    private StigmaSceneController _sc;
    public void Init(DeckUnit targetUnit, List<Stigma> stigmata = null, int stigmaCount = 0, Action afterPopupAction = null, StigmaSceneController sc = null)
    {
        
        _afterPopupAction = afterPopupAction;
        _targetUnit = targetUnit;
        _sc = sc;
        if (targetUnit == null)
        {
            GiveStigmaInit(stigmata);
            return;
        }
        if (stigmata != null)
        {
            List<Stigma> stigmaList = new();
            List<Stigma> existStigma = targetUnit.GetStigma();
            for (int i = 0; i < stigmata.Count; i++)
            {
                if(existStigma.Contains(stigmata[i]))
                {
                    continue;
                }
                else if(stigmata[i] == null)
                {
                    continue;
                }
                else
                {
                    stigmaList.Add(stigmata[i]);
                }
            }
            SetStigmaSelectButtons(stigmaList);
        }
        else
        {
            List<Stigma> stigmaList = CreateStigmaList(_targetUnit, stigmaCount);
            SetStigmaSelectButtons(stigmaList);
        }
    }
    private void GiveStigmaInit(List<Stigma> stigmaList)
    {
        SetStigmaSelectButtons(stigmaList);
    }
    private List<Stigma> CreateStigmaList(DeckUnit targetUnit, int stigmaCount)
    {
        List<Stigma> result = new();
        List<Stigma> existStigma = targetUnit.GetStigma();

        while (result.Count < stigmaCount)
        {
            Stigma stigma = GameManager.Data.StigmaController.GetRandomStigma(GameManager.Data.GetProbability());

            if (existStigma.Contains(stigma))
                continue;

            if (result.Contains(stigma))
                continue;

            result.Add(stigma);
        }

        return result;
    }

    private void SetStigmaSelectButtons(List<Stigma> stigmaList)
    {
        for (int i = 0; i < stigmaList.Count; i++)
        {
            GameObject.Instantiate(_buttonPrefab, _grid).GetComponent<UI_StigmaSelectButton>().Init(stigmaList[i], _sc, this);
        }
    }

    public void OnClick(Stigma stigma)
    {
        if (_targetUnit != null)
            _targetUnit.AddStigma(stigma);

        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
        if (_afterPopupAction != null)
        {
            _afterPopupAction.Invoke();
        }
        GameManager.UI.ClosePopup();
    }

    public void QuitBtn()
    {
        this.transform.SetAsFirstSibling();
        this.gameObject.SetActive(false);
    }
}
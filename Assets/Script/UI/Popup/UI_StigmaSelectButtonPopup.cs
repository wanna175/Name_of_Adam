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

    public void Init(DeckUnit targetUnit, List<Passive> stigmata = null, int stigmaCount = 0, Action afterPopupAction = null)
    {
        _afterPopupAction = afterPopupAction;
        _targetUnit = targetUnit;

        if(stigmata != null)
        {
            List<Passive> stigmaList = new List<Passive>();
            List<Passive> existStigma = targetUnit.Stigma;
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
            List<Passive> stigmaList = CreatePassiveList(_targetUnit, stigmaCount);
            SetStigmaSelectButtons(stigmaList);
        }
    }

    private List<Passive> CreatePassiveList(DeckUnit targetUnit, int stigmaCount)
    {
        List<Passive> result = new List<Passive>();
        List<Passive> existStigma = targetUnit.Stigma;

        while (result.Count < stigmaCount)
        {
            Passive stigma = GameManager.Data.Passive.GetRandomPassive();

            if (existStigma.Contains(stigma))
                continue;

            if (result.Contains(stigma))
                continue;

            result.Add(stigma);
        }

        return result;
    }

    private void SetStigmaSelectButtons(List<Passive> stigmaList)
    {
        for (int i = 0; i < stigmaList.Count; i++)
        {
            GameObject.Instantiate(_buttonPrefab, _grid).GetComponent<UI_StigmaSelectButton>().init(this, stigmaList[i]);
        }
    }

    public void OnClick(Passive stigma)
    {
        _targetUnit.AddStigma(stigma);
        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");

        if(_afterPopupAction != null)
            _afterPopupAction.Invoke();

        GameManager.UI.ClosePopup();
    }
}

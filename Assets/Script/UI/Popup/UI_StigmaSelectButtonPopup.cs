using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StigmaSelectButtonPopup : UI_Popup
{
    private Action _action;
    private DeckUnit _deckUnit;
    [SerializeField] private UI_StigmaSelectButton _buttonPrefab;
    [SerializeField] private Transform _grid;

    public void init(Action<Passive> action,  List<Passive> stigmaList)
    {
        //_action = action;

        for (int i = 0; i < stigmaList.Count; i++)
        { 
            GameObject.Instantiate(_buttonPrefab, _grid).GetComponent<UI_StigmaSelectButton>().init(this, stigmaList[i]);
        }
    }

    public void Init(Action action, BattleUnit targetUnit, int stigmaCount)
    {
        _action = action;
        _deckUnit = targetUnit.DeckUnit;

        List<Passive> stigmaList = CreatePassiveList(targetUnit, stigmaCount);
        SetStigmaSelectButtons(stigmaList);
    }

    private List<Passive> CreatePassiveList(BattleUnit targetUnit, int stigmaCount)
    {
        List<Passive> result = new List<Passive>();
        List<Passive> existStigma = targetUnit.Passive;

        while (result.Count < stigmaCount)
        {
            Passive stigma = GameManager.Data.Passive.GetRandomPassive();

            if (existStigma.Contains(stigma))
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
        _deckUnit.AddStigma(stigma);
        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
        _action.Invoke();
        GameManager.UI.ClosePopup();
    }
}

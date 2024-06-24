using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SortDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    private SortMode _currentSortMode;
    private UI_MyDeck _myDeck;

    public void Init(UI_MyDeck myDeck)
    {
        this._myDeck = myDeck;
        _dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });

        _currentSortMode = SortMode.Default;
        SetSortMode(_currentSortMode);
        SetOptionLocalization();
    }

    private void OnDropdownValueChanged()
    {
        _currentSortMode = (SortMode)_dropdown.value;
        SetSortMode(_currentSortMode);
    }

    public void SetSortMode(SortMode sortMode)
    {
        _currentSortMode = sortMode;
        _dropdown.value = (int)sortMode;

        _myDeck.RefreshDecks(_currentSortMode);
    }

    private void SetOptionLocalization()
    {
        var options = _dropdown.options;
        for (int i = 0; i < options.Count; i++)
            options[i].text = GameManager.Locale.GetLocalizedOption(options[i].text);
    }
}

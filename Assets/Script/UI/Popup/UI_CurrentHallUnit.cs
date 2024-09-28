using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CurrentHallUnit: UI_Popup
{
    [SerializeField] private UI_HallUnitSaveCard _currentCard;

    private DeckUnit _currentUnit;

    public void Init(DeckUnit currentUnit)
    {
        this._currentUnit = currentUnit;
        
        _currentCard.Init(currentUnit);
    }

    public void Save()
    {
        GameManager.OutGameData.AddHallUnit(_currentUnit);
        SceneChanger.SceneChange("MainScene");
    }

    public void QuitButton() => GameManager.UI.ClosePopup(this);

    public void SaveButton() => Save();
}
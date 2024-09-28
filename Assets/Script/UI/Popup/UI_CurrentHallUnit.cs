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

    public void QuitButton() => GameManager.UI.ClosePopup(this);

    public void SaveButton() => SceneChanger.SceneChange("MainScene");
}
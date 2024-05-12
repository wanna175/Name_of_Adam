using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HallUnitSave : UI_Popup
{
    [SerializeField] private UI_HallUnitSaveCard _beforeCard;
    [SerializeField] private UI_HallUnitSaveCard _afterCard;

    private DeckUnit _beforeUnit;
    private DeckUnit _afterUnit;

    public void Init(DeckUnit beforeUnit,  DeckUnit afterUnit)
    {
        this._beforeUnit = beforeUnit;
        this._afterUnit = afterUnit;

        _beforeCard.Init(beforeUnit);
        _afterCard.Init(afterUnit);
    }

    public void QuitButton() => GameManager.UI.ClosePopup(this);

    public void SaveButton() => GameManager.OutGameData.CoverHallUnit(_afterUnit);
}

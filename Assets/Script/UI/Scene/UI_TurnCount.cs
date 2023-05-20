using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TurnCount : UI_Scene
{
    private Text _turnText;

    private void Start()
    {
        _turnText = Util.FindChild<Text>(gameObject, "TurnText", true);
    }

    public void Refresh()
    {
        _turnText.text = BattleManager.Data.TurnCount.ToString();
    }
}

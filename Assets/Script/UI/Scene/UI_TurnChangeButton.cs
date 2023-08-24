using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_TurnChangeButton : UI_Scene
{
    [SerializeField] private Button _changeButton;
    [SerializeField] private TextMeshProUGUI _text;

    public void TurnChange()
    {
        PhaseController _phase = BattleManager.Phase;

        if (_phase.Current == _phase.Prepare)
            _phase.ChangePhase(_phase.Engage);
        else if (_phase.Current == _phase.Move && BattleManager.Data.GetNowUnit().Team == Team.Player)
            _phase.ChangePhase(_phase.Action);
        else if(_phase.Current == _phase.Action && BattleManager.Data.GetNowUnit().Team == Team.Player)
        {
            BattleManager.Data.BattleOrderRemove(BattleManager.Data.GetNowUnit());
            _phase.ChangePhase(_phase.Engage);
        }
        else
            return;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_TurnChangeButton : UI_Scene
{
    [SerializeField] private Button _changeButton;
    [SerializeField] private TextMeshProUGUI _text;

    private bool isCanCtrl;

    public void SetEnable(bool enable)
        => isCanCtrl = enable;

    public void TurnChange()
    {
        GameManager.Sound.Play("Stage_Transition/Engage/EngageEnter");
        
        if (isCanCtrl)
        {
            PhaseController _phase = BattleManager.Phase;
            isCanCtrl = false;

            if (_phase.CurrentPhaseCheck(_phase.Prepare))
                _phase.ChangePhase(_phase.Engage);
            else if (_phase.CurrentPhaseCheck(_phase.Move))
                _phase.ChangePhase(_phase.Action);
            else if (_phase.CurrentPhaseCheck(_phase.Action))
            {
                BattleManager.Data.BattleOrderRemove(BattleManager.Data.GetNowUnit());
                _phase.ChangePhase(_phase.Engage);
            }
        }
    }
}

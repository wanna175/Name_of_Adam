using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase : Phase
{
    public override void OnStateEnter()
    {
        BattleManager.Instance.TurnEnd();
    }
    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Prepare);
    }

    public override void OnClickEvent()
    {
        
    }
    public override void OnStateExit()
    {
        
    }
}

using UnityEngine;

public class BattleOverPhase : Phase
{
    public override void OnStateEnter()
    {
        Debug.Log("끝끝끝");
        BattleManager.BattleUI.UI_turnNotify.Off();
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        
    }
}
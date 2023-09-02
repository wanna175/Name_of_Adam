using System;
using UnityEngine;

public class ActionPhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        _nowUnit = BattleManager.Data.GetNowUnit();
        _nowUnit.AttackTurnStart();

        BattleManager.Field.SetNextActionTileColor(_nowUnit, FieldColorType.Attack);

        if (BattleManager.Data.GetNowUnit().Team == Team.Enemy)
            BattleManager.Instance.PlayAfterCoroutine(_nowUnit.Action.AISkillUse, 1.5f);
    }
    
    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent(Vector2 coord)
    {
        if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            BattleManager.Instance.ActionPhaseClick(coord);
    }

    public override void OnStateExit()
    {
        BattleManager.Field.ClearAllColor();

        _nowUnit.AttackTurnEnd();
        _nowUnit = null;
    }
}
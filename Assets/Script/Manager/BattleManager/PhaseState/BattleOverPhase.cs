using UnityEngine;

public class BattleOverPhase : Phase
{
    public override void OnStateEnter()
    {
        Debug.Log("끝끝끝");
        
        foreach (DeckUnit unit in BattleManager.Data.PlayerDeck)
            unit.FirstTurnDiscountUndo();

        foreach (DeckUnit unit in BattleManager.Data.PlayerHands)
            unit.FirstTurnDiscountUndo();

        BattleManager.BattleUI.UI_turnNotify.Off();
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        
    }
}
using UnityEngine;

public class BattleOverPhase : Phase
{
    public override void OnStateEnter()
    {
        foreach (DeckUnit unit in BattleManager.Data.PlayerDeck)
            unit.FirstTurnDiscountUndo();

        foreach (DeckUnit unit in BattleManager.Data.PlayerHands)
            unit.FirstTurnDiscountUndo();

        BattleManager.BattleUI.RefreshHand();
        BattleManager.Field.ClearAllColor();

        BattleManager.BattleUI.UI_turnNotify.Off();
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        
    }
}
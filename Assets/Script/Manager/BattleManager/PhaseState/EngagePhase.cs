public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.Field.ClearAllColor();
        _battle.Data.BattleUnitOrder();
    }

    public override void OnStateUpdate()
    { 
        _battle.EngagePhase();
    }

    public override void OnStateExit()
    {
        
    }
}
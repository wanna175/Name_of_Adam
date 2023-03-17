public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        if (_battle.Data.OrderUnitCount <= 0)
        {
            _battle.Data.BattleUnitOrder();
        }

        _battle.Field.ClearAllColor();
    }

    public override void OnStateUpdate()
    { 
        _battle.EngagePhase();
    }

    public override void OnStateExit()
    {
        
    }
}
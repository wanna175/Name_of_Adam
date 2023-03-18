public class PreparePhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.Data.TurnPlus();
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateExit()
    {
        _battle.Mana.ChangeMana(2);
        _battle.Data.BattleUnitOrder();
    }
}
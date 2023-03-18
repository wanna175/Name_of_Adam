public class StartPhase : Phase
{
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnClickEvent()
    {
        _battle.PreparePhase();
    }
    public override void OnStateExit()
    {
        _battle.Data.BattleUnitOrder();
    }
}
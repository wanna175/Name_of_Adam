public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {

    }

    public override void OnStateUpdate()
    { 
        _battle.EngagePhase();
    }

    public override void OnStateExit()
    {
        
    }
}
public class SetupPhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.SetupField();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Spawn);
    }

    public override void OnStateExit()
    {
        
    }
}
public class SpawnPhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.SpawnInitialUnit();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Start);
    }

    public override void OnStateExit()
    {
        
    }
}
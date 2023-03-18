public class SpawnPhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.SpawnInitialUnit();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Prepare);
    }

    public override void OnStateExit()
    {
        
    }
}
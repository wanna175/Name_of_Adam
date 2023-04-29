public class SpawnPhase : Phase
{
    public override void OnStateEnter()
    {
        BattleManager.Instance.SpawnInitialUnit();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Prepare);
    }

    public override void OnStateExit()
    {
        
    }
}
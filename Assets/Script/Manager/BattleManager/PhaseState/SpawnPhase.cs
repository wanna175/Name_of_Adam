public class SpawnPhase : Phase
{
    public override void OnStateEnter()
    {
        if (_isFirstExcute == false)
            return;

        _isFirstExcute = false;

        _battle.SpawnInitialUnit();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Start);
    }

    public override void OnStateExit()
    {
        _isFirstExcute = true;
    }
}
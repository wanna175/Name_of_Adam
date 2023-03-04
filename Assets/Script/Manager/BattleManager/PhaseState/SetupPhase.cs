public class SetupPhase : Phase
{
    public override void OnStateEnter()
    {
        if (_isFirstExcute == false)
            return;

        _isFirstExcute = false;

        _battle.SetupField();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Spawn);
    }

    public override void OnStateExit()
    {
        _isFirstExcute = true;
    }
}
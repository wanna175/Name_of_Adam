public class StartPhase : Phase
{
    public override void OnStateEnter()
    {
        if (_isFirstExcute == false)
            return;

        _isFirstExcute = false;
    }

    public override void OnStateUpdate()
    {
        if (IsExit())
            _controller.ChangePhase(_controller.Engage);
    }

    public override void OnStateExit()
    {
        _isFirstExcute = true;
    }

    protected override bool IsExit()
    {
        return _battle._clickType >= ClickType.Engage_Nothing;
    }
}
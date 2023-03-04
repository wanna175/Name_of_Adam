using UnityEngine;

public class BattleOverPhase : Phase
{
    public override void OnStateEnter()
    {
        if (_isFirstExcute == false)
            return;

        _isFirstExcute = false;
        Debug.Log("끝끝끝");
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        _isFirstExcute = true;
    }
}
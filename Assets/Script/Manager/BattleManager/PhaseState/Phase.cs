using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Phase
{
    protected PhaseController _controller;
    protected BattleManager _battle;
    protected bool _isFirstExcute = true;

    public void SetController(PhaseController controller)
    {
        this._controller = controller;
        _battle = GameManager.Battle;
    }

    public abstract void OnStateEnter();
    // 시작할 때 한 번 실행

    public abstract void OnStateUpdate();
    // 페이즈 내내 실행

    public abstract void OnStateExit();
    // (IsExit 충족 시) 끝날 때 한 번 실행 

    protected abstract bool IsExit();
    // 끝나는 조건
}

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
        OnStateEnter();
        OnStateExit();
    }

    public override void OnStateExit()
    {
        _controller.ChangePhase(_controller.Spawn);
        _isFirstExcute = true;
    }

    protected override bool IsExit()
    {
        throw new NotImplementedException();
    }
}

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
        OnStateEnter();
        OnStateExit();
        
    }

    public override void OnStateExit()
    {
        _isFirstExcute = true;
        _controller.ChangePhase(_controller.Start);
    }

    protected override bool IsExit()
    {
        throw new NotImplementedException();
    }
}

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
        OnStateEnter();

        if (IsExit())
            OnStateExit();
            
    }

    public override void OnStateExit()
    {
        _controller.ChangePhase(_controller.Engage);
        _isFirstExcute = true;
    }

    protected override bool IsExit()
    {
        return _battle._clickType >= ClickType.Engage_Nothing;
    }
}

public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        if (_isFirstExcute == false)
            return;

        _isFirstExcute = false;
    }

    public override void OnStateUpdate()
    {
        OnStateEnter();

        _battle.EngagePhase();
    }

    public override void OnStateExit()
    {
        _isFirstExcute = true;
    }

    protected override bool IsExit()
    {
        throw new NotImplementedException();
    }
}

public class PreparePhase : Phase
{
    public override void OnStateEnter()
    {
        if (_isFirstExcute == false)
            return;

        _isFirstExcute = false;
    }

    public override void OnStateUpdate()
    {
        OnStateEnter();

        if(IsExit())   
            OnStateExit();
    }

    public override void OnStateExit()
    {
        _battle.Mana.ChangeMana(2);
        _battle.Data.TurnPlus();

        _controller.ChangePhase(_controller.Engage);

        _isFirstExcute = true;
    }

    protected override bool IsExit()
    {
        return _battle._clickType >= ClickType.Engage_Nothing;
    }
}

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
        OnStateEnter();
    }

    public override void OnStateExit()
    {
        _isFirstExcute = true;
    }

    protected override bool IsExit()
    {
        throw new NotImplementedException();
    }
}
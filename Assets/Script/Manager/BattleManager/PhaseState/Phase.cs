using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Phase
{
    protected PhaseController _controller;
    protected BattleManager _battle;

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

    protected virtual bool IsExit()
    {
        return true;
    }
    // 끝나는 조건
}
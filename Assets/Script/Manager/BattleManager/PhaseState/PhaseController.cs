using System;
using UnityEngine;

public class PhaseController
{
    private Phase _currentPhase = null;
    public Phase Current => _currentPhase;
    public Phase Setup, Spawn, Engage, Move, Action, Prepare, End, BattleOver;

    public PhaseController()
    {
        InitializePhase();
        ChangePhase(Setup);
    }

    public void OnUpdate()
    {
        _currentPhase.OnStateUpdate();
    }

    public void OnClickEvent(Vector2 coord)
    {
        _currentPhase.OnClickEvent(coord);
    }

    public void ChangePhase(Phase nextPhase)
    {
        if (_currentPhase != null)
            _currentPhase.OnStateExit();

        _currentPhase = nextPhase;
        _currentPhase.SetController(this);
        _currentPhase.OnStateEnter();
    }

    private void InitializePhase()
    {
        Setup = new SetupPhase();
        Spawn = new SpawnPhase();
        Engage = new EngagePhase();
        Move = new MovePhase();
        Action = new ActionPhase();
        Prepare = new PreparePhase();
        End = new EndPhase();
        BattleOver = new BattleOverPhase();
    }
}
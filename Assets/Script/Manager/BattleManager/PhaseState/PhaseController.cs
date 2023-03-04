using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PhaseController
{
    private Phase _currentPhase = null;
    public Phase Current => _currentPhase;
    public Phase Setup, Spawn, Start, Engage, Prepare, BattleOver;

    public PhaseController()
    {
        InitializePhase();
        ChangePhase(Setup);
    }

    public void OnUpdate()
    {
        _currentPhase.OnStateUpdate();
    }

    public void ChangePhase(Phase nextPhase)
    {
        _currentPhase = nextPhase;
        _currentPhase.SetController(this);
    }

    private void InitializePhase()
    {
        Setup = new SetupPhase();
        Spawn = new SpawnPhase();
        Start = new StartPhase();
        Engage = new EngagePhase();
        Prepare = new PreparePhase();
        BattleOver = new BattleOverPhase();
    }
}
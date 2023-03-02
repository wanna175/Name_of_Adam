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

    public abstract void OnUpdate();
}

public class SetupPhase : Phase
{
    public override void OnUpdate()
    {
        _battle.SetupField();

        _controller.ChangePhase(_controller.Spawn);
    }
}

public class SpawnPhase : Phase
{
    public override void OnUpdate()
    {
        _battle.SpawnInitialUnit();

        _controller.ChangePhase(_controller.Start);
    }
}

public class StartPhase : Phase
{
    public override void OnUpdate()
    {
        if (_battle._clickType >= ClickType.Engage_Nothing)
        {
            _controller.ChangePhase(_controller.Engage);
        }
    }
}

public class EngagePhase : Phase
{
    public override void OnUpdate()
    {
        _battle.EngagePhase();
    }
}

public class PreparePhase : Phase
{
    public override void OnUpdate()
    {
        Debug.Log("Prepare Enter");

        _battle.Mana.ChangeMana(2);
        _battle.Data.TurnPlus();

        if (_battle._clickType >= ClickType.Engage_Nothing)
        {
            //PrepareExit();
            Debug.Log("Prepare Exit");

            _controller.ChangePhase(_controller.Engage);
        }
    }
}

public class BattleOverPhase : Phase
{
    public override void OnUpdate()
    {
        Debug.Log("끝끝끝");
    }
}
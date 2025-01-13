using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_RahelLea : UnitAction
{
    private BattleUnit _owner = null;
    private bool _isChanged = false;
    const int _speedDifference = 65;

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count == 0)
            return false;

        if (_isChanged)
        {
            List<BattleUnit> units = BattleManager.Field.GetArroundUnits(attackUnit.Location, attackUnit.Team == Team.Player ? Team.Enemy : Team.Player);
            BattleManager.Instance.AttackStart(attackUnit, units.Distinct().ToList());
            GameManager.Instance.PlayAfterCoroutine(() => GameManager.Sound.Play("Character/" + _owner.Data.ID + "/����_Attack"), 0.65f);
        }
        else 
        {
            BattleManager.Instance.AttackStart(attackUnit, hits);
            GameManager.Instance.PlayAfterCoroutine(() => GameManager.Sound.Play("Character/" + _owner.Data.ID + "/����_Attack"), 1.0f);
        }

        return true;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            _owner = caster;
            _owner.SetBuff(new Buff_Rahel());
            InsertUnitInOrlder();
        }
        else if ((activeTiming & ActiveTiming.TURN_START) == ActiveTiming.TURN_START)
        {
            InsertUnitInOrlder();
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END)
        {
            ChangeState(caster);
        }
        else if ((activeTiming & ActiveTiming.MOVE_TURN_START) == ActiveTiming.MOVE_TURN_START)
        {
            if (_isChanged)
                return true;
        }

        return false;
    }

    private void InsertUnitInOrlder()
    {
        if ( ! BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
        {
            return;
        }

        int ChangedSpeed = _owner.BattleUnitTotalStat.SPD - _speedDifference;

        BattleManager.Data.BattleOrderInsert(0, _owner, ChangedSpeed);
    }

    private void ChangeState(BattleUnit caster)
    {
        _isChanged = !_isChanged;

        caster.AnimatorSetBool("isChanged", _isChanged);

        if (_isChanged)
        {
            _owner.DeleteBuff(BuffEnum.Rahel);
            _owner.SetBuff(new Buff_Lea());
            _owner.SkillEffectAnim = GameManager.Resource.Load<AnimationClip>("Arts/EffectAnimation/AttackEffect/Lea_AttackEffect");
        }
        else
        {
            _owner.DeleteBuff(BuffEnum.Lea);
            _owner.SetBuff(new Buff_Rahel());
            _owner.SkillEffectAnim = GameManager.Resource.Load<AnimationClip>("Arts/EffectAnimation/AttackEffect/Rahel_AttackEffect");
        }
    }

    public override List<Vector2> GetSplashRangeForField(BattleUnit unit, Tile targetTile, Vector2 caster)
    {
        List<Vector2> splashRangeList = new();
        Vector2 target = BattleManager.Field.GetCoordByTile(targetTile);

        if (_isChanged)
        {
            foreach (Vector2 vec in unit.GetAttackRange())
            {
                if (BattleManager.Field.IsInRange(vec + caster))
                    splashRangeList.Add(vec + caster);
            }
        }
        else
        {
            splashRangeList.Add(target);
        }

        return splashRangeList;
    }
}
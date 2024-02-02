using UnityEngine;

public class Buff_Leah : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Leah;

        _name = "레아";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Tailwind_Sprite");

        _description = "레아.";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK -= 10;

        return stat;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        if (caster.Buff.CheckBuff(BuffEnum.MarkOfRaquel))
        {
            caster.DeleteBuff(BuffEnum.MarkOfRaquel);
            caster.ChangeFall(1);

            if (caster.FallEvent)
            {
                return true;
            }
            else
            {
                caster.GetAttack(-10, _owner);
            }
        }

        return false;
    }
}
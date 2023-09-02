using UnityEngine;

public class Buff_DeathStrike : Buff
{
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.DeathStrike;

        _name = "결사의 일격";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_DeathStrike_Sprite");

        _description = "결사의 일격.";

        _count = 1;

        _countDownTiming = ActiveTiming.DAMAGE_CONFIRM;

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        caster.ChangedDamage *= 3;

        Buff_InevitableEnd inevitableEnd= new();
        caster.SetBuff(inevitableEnd, caster);

        return false;
    }
}
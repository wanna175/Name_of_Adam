using UnityEngine;

public class Buff_DeathStrike : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.DeathStrike;

        _name = "Death Strike";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_DeathStrike_Sprite");

        _description = "Damage triples, but dies after attacking.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.ChangedDamage *= 3;

        _owner.SetBuff(new Buff_AfterMotionTransparent());
        _owner.SetBuff(new Buff_AfterAttackDead());

        return false;
    }
}
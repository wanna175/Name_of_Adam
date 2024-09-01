using UnityEngine;

public class Buff_DeathStrike : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.DeathStrike;

        _name = "Death Strike";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_DeathStrike_Sprite");

        _description = "Death Strike Info";

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _owner = owner;

        _dispellable = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.ChangedDamage *= 3;

        _owner.SetBuff(new Buff_AfterMotionTransparent());
        _owner.SetBuff(new Buff_AfterAttackDead());

        return false;
    }
}
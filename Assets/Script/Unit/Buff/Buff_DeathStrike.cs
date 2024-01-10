using UnityEngine;

public class Buff_DeathStrike : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.DeathStrike;

        _name = "결사의 일격";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_DeathStrike_Sprite");

        _description = "3배의 대미지를 입히는 대신 공격 후 사망합니다.";

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

        Buff_AfterAttackDead afterAttackDead = new();
        _owner.SetBuff(afterAttackDead);

        return false;
    }
}
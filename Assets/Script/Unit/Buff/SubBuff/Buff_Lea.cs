using UnityEngine;

public class Buff_Lea : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Lea;

        _name = "Lea";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Lea_Sprite");

        _description = "Lea Info";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = true;

        _isSystemBuff = true;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        //stat.ATK -= 5;

        return stat;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        if (caster.Buff.CheckBuff(BuffEnum.MarkOfBeast))
        {
            caster.DeleteBuff(BuffEnum.MarkOfBeast);
            caster.ChangeFall(1, _owner, FallAnimMode.On);
        }

        return false;
    }
}
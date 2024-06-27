using UnityEngine;

public class Buff_Grudge : Buff
{
    private int _attackUp = 0;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Grudge;

        _name = "Grudge";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Raise_Sprite");

        _description = "Grudge Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = true;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();  
        stat.ATK += _attackUp;

        return stat;
    }

    public override void SetValue(int num) 
    {
        _attackUp = num;
    }
}
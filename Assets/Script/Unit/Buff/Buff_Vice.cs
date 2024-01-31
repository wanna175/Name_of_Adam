using Unity.VisualScripting;
using UnityEngine;

public class Buff_Vice : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Vice;

        _name = "¾Ç¼º";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Vice_Sprite");

        _description = "°ø°Ý ½Ã ÀûÀÇ ½Å¾ÓÀ» 1 ¶³¾î¶ß¸³´Ï´Ù.";

        _count = 1;

        _countDownTiming = ActiveTiming.BEFORE_ATTACK;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.ChangeFall(1);

        return false;
    }
}
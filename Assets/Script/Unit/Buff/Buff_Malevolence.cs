using Unity.VisualScripting;
using UnityEngine;

public class Buff_Malevolence : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Malevolence;

        _name = "Malevolence";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Malevolence_Sprite");

        _description = "Malevolence Info";

        _count = 1;

        _countDownTiming = ActiveTiming.BEFORE_ATTACK;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _dispellable = true;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override void SetValue(int num)
    {
        _count = num;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.ChangeFall(1, _owner, FallAnimMode.On);

        return false;
    }
}
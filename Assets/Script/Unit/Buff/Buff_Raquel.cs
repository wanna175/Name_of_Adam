using UnityEngine;

public class Buff_Raquel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Raquel;

        _name = "Rahel";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Raquel_Sprite");

        _description = "Rahel Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        if (_owner.AttackUnitNum == 1)
        {
            BattleManager.Instance.PlayAfterCoroutine(() =>
            {
                Vector2 vec = caster.Location + (caster.Location - _owner.Location).normalized;
                if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
                {
                    BattleManager.Instance.MoveUnit(_owner, vec);
                    _owner.SetFlipX(!_owner.GetFlipX());
                }
            }, 0.5f);
        }

        if (caster.Buff.CheckBuff(BuffEnum.MarkOfBeast))
        {
            caster.DeleteBuff(BuffEnum.MarkOfBeast);
            caster.ChangeFall(1, FallAnimMode.On, 0.75f);
        }

        caster.SetBuff(new Buff_MarkOfBeast());

        return false;
    }
}
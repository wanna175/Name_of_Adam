using UnityEngine;

public class Buff_Divine : Buff
{
    private GameObject _divineEffect;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Divine;

        _name = "Divine";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Divine_Sprite");

        _description = "Divine Info";

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _owner = owner;

        _divineEffect = GameManager.VisualEffect.StartDivineEffect(_owner);
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.ChangeFall(1, FallAnimMode.On);

        return false;
    }

    public override void Destroy()
    {
        Destroy(_divineEffect);
    }
}
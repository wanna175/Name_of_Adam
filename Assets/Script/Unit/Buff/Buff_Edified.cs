using UnityEngine;

public class Buff_Edified : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Edified;

        _name = "Edified";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Edified_Sprite");

        _description = "Edified Info";

        _owner = owner;
    }
}
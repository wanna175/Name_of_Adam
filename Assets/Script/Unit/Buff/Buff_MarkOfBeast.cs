using UnityEngine;

public class Buff_MarkOfBeast : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.MarkOfBeast;

        _name = "MarkOfTheBeast";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_MarkOfBeast_Sprite");

        _description = "MarkOfTheBeast Info";

        _owner = owner;

        _dispellable = true;

        _isDebuff = true;
    }
}
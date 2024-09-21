using UnityEngine;

public class Buff_Scale : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Scale;

        _name = "Scale";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_BlessingOfYohrn_Sprite");

        _description = "Scale Info";

        _owner = owner;

        _statBuff = true;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class Buff_Dusk  : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Dusk;

        _name = "Dusk";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Dusk_Sprite");

        _description = "Dusk Info";

        _count = 1;

        _owner = owner;

        _isDebuff = true;
    }

    public override void Stack()
    {
        _count += 1;
    }
}
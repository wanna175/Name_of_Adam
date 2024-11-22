using System.Collections.Generic;
using UnityEngine;

public class Buff_Despair : Buff
{
    private Stigma _stigmata;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Despair;

        _name = "Despair";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Despair_Sprite");

        _description = "Despair Info";

        _owner = owner;

        _isSystemBuff = true;
    }

    public void SetStigmata(Stigma stigmata)
    {
        _stigmata = stigmata;
    }

    public override string GetDescription(int spacing = 1)
    {
        string desc = "<color=#FF9696><size=110%><b>" + _stigmata.Name + "(" + Name + ")" + "</b><color=white></size>";
        for (int i = 0; i < spacing; i++)
            desc += "\n";
        desc += _stigmata.Description;
        desc += "\n";
        desc += GameManager.Locale.GetLocalizedBuffInfo(_description);

        return desc;
    }
}
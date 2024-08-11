using UnityEngine;
using System.Collections.Generic;

public class Buff_Appaim : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Appaim;

        _name = "Appaim";

        _description = "Appaim Info";

        _owner = owner;
    }

    private int _buffState = 0;
    private Dictionary<int, string> _nameDict = new() { { 0, "Book" }, {1, "Staff"}, { 2, "Sword" } };
    private Dictionary<int, string> _descriptionDict = new() {
        { 0, "Book Info" },
        { 1, "Staff Info" },
        { 2, "Sword Info" }
    };


    public override string GetDescription(int spacing = 1)
    {
        string desc = "<size=110%><b>" + GameManager.Locale.GetLocalizedBuffName(_nameDict[_buffState]) + "</b></size>";
        for (int i = 0; i < spacing; i++)
            desc += "\n";
        desc += GameManager.Locale.GetLocalizedBuffInfo(_descriptionDict[_buffState]);

        return desc;
    }

    public override void SetValue(int num)
    {
        _buffState = num;
        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Appaim_{_nameDict[_buffState]}_Sprite");
    }
}
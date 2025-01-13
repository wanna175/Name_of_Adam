using System.Collections.Generic;
using UnityEngine;

public class Buff_Libiel : Buff
{
    private int _libielCount = 1;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Libiel;

        _name = "Libiel";

        _description = "Libiel Info";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Libiel_Sprite");

        _owner = owner;

        _isSystemBuff = true;
    }

    public override void SetValue(int num) => _libielCount = num;
    public override int GetBuffDisplayNumber() => _libielCount;

    private Dictionary<int, string> _descriptionDict = new() {
        { 1, "Glory1 Info" },
        { 2, "Glory2 Info" },
        { 3, "Glory3 Info" }
    };


    public override string GetDescription(int spacing = 1)
    {
        string desc = "<size=110%><b>" + GameManager.Locale.GetLocalizedBuffName("Glory") + "</b></size>";
        for (int i = 0; i < spacing; i++)
            desc += "\n";
        desc += GameManager.Locale.GetLocalizedBuffInfo(_descriptionDict[_libielCount]);

        return desc;
    }
}
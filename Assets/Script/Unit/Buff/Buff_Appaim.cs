using UnityEngine;
using System.Collections.Generic;

public class Buff_Appaim : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Raquel;

        _name = "æ–πŸ¿”";

        _description = "";

        //_sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Appaim_Sprite");

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }

    private int _buffState = 0;
    private Dictionary<int, string> _nameDict = new() { { 0, "Book" }, {1, "Staff"}, { 2, "Sword" } };
    private Dictionary<int, string> _descriptionDict = new() {
        { 0, "Attacks in an area effect, and becomes immobile but decreases the enemy's faith by 1." },
        { 1, "Attacks in an area effect." },
        { 2, "Attacking range becomes infinite." }
    };


    public override string GetDescription()
    {
        string desc = "<size=110%><b>" + GameManager.Locale.GetLocalizedBuffName(_nameDict[_buffState]) + "</b></size>\n<size=30%>\n</size>" + GameManager.Locale.GetLocalizedBuffInfo(_descriptionDict[_buffState]); ;

        return desc;
    }

    public override void SetValue(int num)
    {
        _buffState = num;
        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Appaim_{_nameDict[_buffState]}_Sprite");
    }
}
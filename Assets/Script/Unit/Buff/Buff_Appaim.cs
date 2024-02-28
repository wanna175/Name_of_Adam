using UnityEngine;
using System.Collections.Generic;

public class Buff_Appaim : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Raquel;

        _name = "æ–πŸ¿”";

        _description = "";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Appaim_Sprite");

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }

    private int _buffState = 0;
    private Dictionary<int, string> _nameDict = new() { {0, "Sword"}, {1, "Staff"}, {2, "Book"} };
    private Dictionary<int, string> _descriptionDict = new() { 
        { 0, "Attacks in an area effect." }, 
        { 1, "Attacking range becomes infinite." }, 
        { 2, "Attacks in an area effect, and becomes immobile but decreases the enemy's faith by 1." } 
    };


    public override string GetDescription()
    {
        string desc = "<size=110%><b>" + GameManager.Locale.GetLocalizedBuffName(_nameDict[_buffState]) + "</b></size>\n<size=30%>\n</size>" + GameManager.Locale.GetLocalizedBuffInfo(_descriptionDict[_buffState]); ;

        return desc;
    }

    public override void SetValue(int num)
    {
        _buffState = num;
    }
}
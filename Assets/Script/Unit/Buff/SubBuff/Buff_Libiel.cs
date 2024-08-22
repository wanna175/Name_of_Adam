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
}
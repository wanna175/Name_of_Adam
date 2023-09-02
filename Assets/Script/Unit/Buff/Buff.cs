using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    protected BuffEnum _buffEnum;
    public BuffEnum BuffEnum => _buffEnum;

    protected string _name;
    public string Name => _name;

    protected Sprite _sprite;
    public Sprite Sprite => _sprite;

    protected string _description;

    protected int _count;
    public int Count => _count;

    protected ActiveTiming _countDownTiming;
    public ActiveTiming CountDownTiming => _countDownTiming;

    protected ActiveTiming _buffActiveTiming;
    public ActiveTiming BuffActiveTiming => _buffActiveTiming;

    protected BattleUnit _caster;
    public BattleUnit Caster => _caster;

    protected BattleUnit _owner;
    public BattleUnit Owner => _owner;

    protected bool _statBuff;
    public bool StatBuff => _statBuff;

    protected bool _dispellable;
    public bool Dispellable => _dispellable;

    protected bool _stigmaBuff;
    public bool StigmaBuff => _stigmaBuff;

    public abstract void Init(BattleUnit caster, BattleUnit owner);

    public virtual bool Active(BattleUnit caster = null, BattleUnit receiver = null)
    {
        return false;
    }

    public virtual void Stack()
    {
    }

    public virtual Stat GetBuffedStat()
    {
        Stat stat = new();

        return stat;
    }

    public virtual void SetValue(int num)
    { 
    }

    public virtual int GetBuffDisplayNumber()
    {
        if (_count > 0)
            return _count;
        else
            return 0;
    }

    public virtual string GetDescription()
    {
        return "<size=110%><b>" + _name + "</b></size>\n<size=30%>\n</size>" + _description;
    }

    public void CountChange(int num)
    {
        _count += num;
    }
}
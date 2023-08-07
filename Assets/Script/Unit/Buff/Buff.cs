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
    public string Description => _description;

    protected int _count;
    public int Count => _count;

    protected ActiveTiming _countDownTiming;
    public ActiveTiming CountDownTiming => _countDownTiming;

    protected ActiveTiming _buffActiveTiming;
    public ActiveTiming BuffActiveTiming => _buffActiveTiming;

    protected bool _statBuff;
    public bool StatBuff => _statBuff;

    protected bool _dispellable;
    public bool Dispellable => _dispellable;

    protected bool _caster;
    public bool Caster => _caster;

    protected bool _owner;
    public bool Owner => _owner;

    public abstract void Init(BattleUnit caster, BattleUnit owner);

    public virtual bool Active(BattleUnit caster = null, BattleUnit receiver = null)
    {
        Debug.Log("BUFF");

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

    public virtual void CountChange(int num)
    {
        _count += num;
    }
}
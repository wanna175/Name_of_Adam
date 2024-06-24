using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    protected BuffEnum _buffEnum;
    public BuffEnum BuffEnum => _buffEnum;

    protected string _name;
    public string Name => GameManager.Locale.GetLocalizedBuffName(_name);

    protected Sprite _sprite;
    public Sprite Sprite => _sprite;

    protected string _description;

    protected int _count;
    public int Count => _count;

    protected ActiveTiming _countDownTiming;
    public ActiveTiming CountDownTiming => _countDownTiming;

    protected ActiveTiming _buffActiveTiming;
    public ActiveTiming BuffActiveTiming => _buffActiveTiming;

    protected BattleUnit _owner;
    public BattleUnit Owner => _owner;

    protected bool _statBuff;

    public bool StatBuff => _statBuff;

    protected bool _dispellable;

    public bool Dispellable => _dispellable;

    protected bool _stigmaBuff;

    public bool StigmaBuff => _stigmaBuff;


    public abstract void Init(BattleUnit owner);

    public virtual bool Active(BattleUnit caster = null) => false;

    public virtual Stat GetBuffedStat() => new Stat();

    public virtual void SetValue(int num) { }

    public virtual void Stack() { }

    public virtual int GetBuffDisplayNumber()
    {
        if (_count > 0) 
            return _count;
        else 
            return 0;
    }

    public virtual void Destroy() { }

    public void CountChange(int num) => _count += num;

    public virtual string GetDescription(int spacing = 1)
    {
        string desc = "<color=#FF9696><size=110%><b>" + Name + "</b><color=white></size>";
        for (int i = 0; i < spacing; i++)
            desc += "\n";
        desc += GameManager.Locale.GetLocalizedBuffInfo(_description);

        if (_countDownTiming != ActiveTiming.NONE)
        {
            if (GameManager.Locale.CurrentLocaleIndex == 0)
                desc += " (Remaining: " + _count.ToString() + ")";
            else
                desc += " (남은 횟수: " + _count.ToString() + ")";
        }    

        return desc;
    }
}
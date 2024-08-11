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

    protected int _count = -1;
    public int Count => _count;

    protected ActiveTiming _countDownTiming = ActiveTiming.NONE;
    public ActiveTiming CountDownTiming => _countDownTiming;

    protected ActiveTiming _buffActiveTiming = ActiveTiming.NONE;
    public ActiveTiming BuffActiveTiming => _buffActiveTiming;

    protected BattleUnit _owner;
    public BattleUnit Owner => _owner;

    protected bool _statBuff = false;
    public bool StatBuff => _statBuff;

    protected bool _dispellable = false;
    public bool Dispellable => _dispellable;

    protected bool _stigmataBuff = false;
    public bool StigmataBuff => _stigmataBuff;

    protected bool _isDebuff = false;
    public bool IsDebuff => _isDebuff;

    protected bool _isActive = false;
    public bool IsActive => _isActive;

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
            return -1;
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
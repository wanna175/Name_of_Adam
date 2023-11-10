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

    /// <summary> 버프 횟수 </summary>
    public int Count => _count;

    protected ActiveTiming _countDownTiming;

    /// <summary> 버프 횟수 차감 시점 </summary>
    public ActiveTiming CountDownTiming => _countDownTiming;

    protected ActiveTiming _buffActiveTiming;


    /// <summary> 버프 효과 발동 시점 </summary>
    public ActiveTiming BuffActiveTiming => _buffActiveTiming;


    protected BattleUnit _owner;

    /// <summary> 버프를 보유한 유닛 </summary>
    public BattleUnit Owner => _owner;

    protected bool _statBuff;


    /// <summary> 이 버프는 스탯을 올려주는 버프인지에 대한 여부 </summary>
    public bool StatBuff => _statBuff;

    protected bool _dispellable;


    /// <summary> 이 버프는 해제가 가능한지에 대한 여부 </summary>
    public bool Dispellable => _dispellable;

    protected bool _stigmaBuff;


    /// <summary> 이 버프는 낙인 버프인지에 대한 여부 </summary>
    public bool StigmaBuff => _stigmaBuff;


    /// <summary> 버프 데이터 설정 함수 </summary>
    /// <param name="owner"> 버프 소유 유닛 </param>
    public abstract void Init(BattleUnit owner);


    /// <summary> 버프 효과 수행 함수 </summary>
    /// <param name="caster"> 버프 효과를 받을 유닛 </param>
    /// <returns> 해당 버프로 인해 다음 페이즈가 넘어가는지에 대한 여부 </returns>
    public virtual bool Active(BattleUnit caster = null) => false;


    /// <summary> 버프 효과에 대한 스탯 반환 함수 </summary>
    /// <returns> 버프 효과를 받은 스탯 </returns>
    public virtual Stat GetBuffedStat() => new Stat();


    /// <summary> 스탯 같은 특정 int 값에 대해 설정하는 함수 </summary>
    /// <param name="num">스탯 같은 특정 int 값</param>
    public virtual void SetValue(int num) { }


    /// <summary> 만약 현재 유닛에 이미 버프가 존재하면 수행되는 함수 </summary>
    public virtual void Stack() { }


    /// <summary> 출력을 위한 버프 남은 횟수를 반환하는 함수 </summary>
    /// <returns> 남은 버프 효과 횟수 </returns>
    public virtual int GetBuffDisplayNumber()
    {
        if (_count > 0) return _count;
        else return 0;
    }


    /// <summary> 버프가 유닛에서 삭제될 때 호출되는 함수 </summary>
    public virtual void Destroy() { }


    /// <summary> 버프 횟수를 'num'만큼 더하는 함수 함수 </summary>
    /// <param name="num"> 변동 수치 </param>
    public void CountChange(int num) => _count += num;


    /// <summary> 버프 설명 문자열을 반환하는 함수 </summary>
    /// <returns> 버프 설명 문자열 </returns>
    public virtual string GetDescription()
    {
        string desc = "<size=110%><b>" + _name + "</b></size>\n<size=30%>\n</size>" + _description;

        if (_countDownTiming != ActiveTiming.NONE)
            desc += " (" + _count.ToString() + "회)";

        return desc;
    }
}
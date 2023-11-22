using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public DeckUnit DeckUnit;
    public UnitDataSO Data => DeckUnit.Data;

    [SerializeField] public Stat BattleUnitChangedStat; // 버프 등으로 변경된 스탯
    public Stat BattleUnitTotalStat => DeckUnit.DeckUnitTotalStat + BattleUnitChangedStat; // 실제 적용 중인 스탯

    [SerializeField] private Team _team;
    public Team Team => _team;

    private SpriteRenderer _renderer;
    private Animator _unitAnimator;
    public AnimationClip SkillEffectAnim;

    //[SerializeField] public UnitAIController AI;
    [SerializeField] public UnitHP HP;
    [SerializeField] public UnitFall Fall;
    [SerializeField] public UnitBuff Buff;
    [SerializeField] public UnitAction Action;
    [SerializeField] private UI_HPBar _hpBar;
    [SerializeField] private UI_FloatingDamage _floatingDamage;

    [SerializeField] public List<Stigma> StigmaList => DeckUnit.GetStigma();

    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;
    public bool FallEvent = false;

    private float _scale;
    private float GetModifiedScale() => _scale + ((_scale * 0.1f) * (_location.y - 1));

    private bool[] _moveRangeList;
    private bool[] _attackRangeList;

    private IEnumerator moveCoro;

    public bool IsConnectedUnit;
    public List<ConnectedUnit> ConnectedUnits;

    public bool NextMoveSkip = false;
    public bool NextAttackSkip = false;

    public void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _unitAnimator = GetComponent<Animator>();

        _renderer.sprite = Data.Image;
        _renderer.color = new Color(1, 1, 1, 1);

        HP.Init(BattleUnitTotalStat.MaxHP, BattleUnitTotalStat.CurrentHP);
        Fall.Init(BattleUnitTotalStat.FallCurrentCount, BattleUnitTotalStat.FallMaxCount);

        ConnectedUnits = new();

        Action = BattleManager.Data.GetUnitAction(Data.UnitActionType);
        Action.Init();

        _hpBar.RefreshHPBar(HP.FillAmount());

        _scale = transform.localScale.x;

        _moveRangeList = new bool[Data.MoveRange.Length];
        Array.Copy(Data.MoveRange, _moveRangeList, Data.MoveRange.Length);

        _attackRangeList = new bool[Data.AttackRange.Length];
        Array.Copy(Data.AttackRange, _attackRangeList, Data.AttackRange.Length);
        
        BattleManager.Data.BattleUnitList.Add(this);

        GameManager.Sound.Play("Summon/SummonSFX");
    }

    public void UnitSetting(Vector2 coord, Team team, bool isConnectedUnit = false)
    {
        SetTeam(team);
        BattleManager.Field.EnterTile(this, coord);
        SetLocation(coord);

        IsConnectedUnit = isConnectedUnit;

        if (!isConnectedUnit)
        {
            if (DeckUnit.GetUnitSize() > 1)
            {
                foreach (Vector2 range in DeckUnit.GetUnitSizeRange())
                {
                    if (range + _location != _location)
                        ConnectedUnits.Add(BattleManager.Spawner.ConnectedUnitSpawn(this, range + _location));
                }
            }

            SetFlipX(team == Team.Enemy);
        }

        //소환 시 체크
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.STIGMA, this);
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.SUMMON, this);
        BattleManager.Instance.UnitSummonEvent();
    }

    public void FieldUnitDead()
    {
        //필드 유닛 사망시 체크
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.FIELD_UNIT_DEAD, this);
    }

    public void FieldUnitSummon()
    {
        //필드 유닛 소화시 체크
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.FIELD_UNIT_SUMMON, this);
    }

    public void SetTeam(Team team)
    {
        _team = team;

        SetHPBar();
        ChangeAnimator();
    }

    public void SetFlipX(bool flip)
    {
        //true -> look left, false -> look right
        if (_renderer.flipX == flip)
            return;

        _renderer.flipX = flip;
        _floatingDamage.DirectionChange(flip);

        if (ConnectedUnits.Count > 0)
        {
            int maxX = (int)_location.x;
            int minX = (int)_location.x;

            foreach (ConnectedUnit unit in ConnectedUnits)
            {
                if (unit.Location.x < minX)
                    minX = (int)unit.Location.x;
                if (unit.Location.x > maxX)
                    maxX = (int)unit.Location.x;
            }

            int size = maxX - minX + 1;

            for (int i = 0; i < size; i++)
            {
                if ((int)_location.x == minX + i)
                {
                    BattleManager.Field.ExitTile(_location);
                    SetLocation(new(maxX - i, _location.y));
                }

                foreach (ConnectedUnit unit in ConnectedUnits)
                {
                    if ((int)unit.Location.x == minX + i)
                    {
                        BattleManager.Field.ExitTile(unit.Location);
                        unit.SetLocation(new(maxX - i, unit.Location.y));
                    }
                }
            }

            foreach (ConnectedUnit unit in ConnectedUnits)
                BattleManager.Field.EnterTile(unit, unit.Location);
            BattleManager.Field.EnterTile(this, this.Location);
        }
    }

    public bool GetFlipX() => _renderer.flipX;

    public void SetHPBar()
    {
        _hpBar.SetHPBar(_team);
        _hpBar.SetFallBar(DeckUnit);
    }

    public void SetLocation(Vector2 coord)
    {
        _location = coord;

        float scale = GetModifiedScale();

        transform.position = BattleManager.Field.GetTilePosition(coord);
        transform.localScale = new(scale, scale, 1);
    }

    public void UnitDiedEvent()
    {
        //자신이 사망 시 체크
        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.BEFORE_UNIT_DEAD, this))
        {
            return;
        }

        BattleManager.Instance.UnitDeadEvent(this);
        foreach (ConnectedUnit unit in ConnectedUnits)
        {
            BattleManager.Instance.UnitDeadEvent(unit);
            BattleManager.Spawner.RestoreUnit(unit.gameObject);
        }
        GameManager.VisualEffect.StartUnitDeadEffect(transform.position, GetFlipX());
        StartCoroutine(UnitDeadEffect());
        GameManager.Sound.Play("Dead/DeadSFX");
    }

    private IEnumerator UnitDeadEffect()
    {
        Color c = _renderer.color;

        while (c.a > 0)
        {
            c.a -= 0.01f;

            _renderer.color = c;

            yield return null;
        }
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.AFTER_UNIT_DEAD, this);
        BattleManager.Spawner.RestoreUnit(gameObject);

        if (BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
        {
            BattleManager.Instance.BattleOverCheck();
        }
    }

    public void UnitFallEvent()
    {
        //타락 시 체크
        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.FALLED, this))
        {
            return;
        }

        GameManager.Data.GameData.FallenUnits.Add(DeckUnit);

        //타락 이벤트 시작
        FallEvent = true;

        GameManager.Sound.Play("UI/FallSFX/Fall");
        GameManager.VisualEffect.StartCorruptionEffect(this, transform.position);
    }

    public void Corrupted()
    {
        //타락 이벤트 종료
        if (ChangeTeam() == Team.Enemy)
        {
            Fall.Editfy();
        }
        FallEvent = false;

        HP.Init(DeckUnit.DeckUnitTotalStat.MaxHP, DeckUnit.DeckUnitTotalStat.MaxHP);
        SetHPBar();
        
        _hpBar.RefreshHPBar(HP.FillAmount());

        DeckUnit.DeckUnitChangedStat.CurrentHP = 0;
        DeckUnit.DeckUnitUpgradeStat.FallCurrentCount = 0;
        if (BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
        {
            BattleManager.Instance.BattleOverCheck();
        }
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.STIGMA, this);

        if (Buff.CheckBuff(BuffEnum.Benediction))
        {
            DeleteBuff(BuffEnum.Benediction);
        }
    }

    //애니메이션에서 직접 실행시킴
    public void AnimAttack()
    {
        BattleManager.BattleCutScene.IsAttack = true;
    }

    public void AnimatorSetBool(string varName, bool boolean)
    {
        _unitAnimator.SetBool(varName, boolean);
    }

    public Team ChangeTeam(Team team = default)
    {
        if (team != default)
        {
            SetTeam(team);
            return team;
        }

        if (Team == Team.Player)
        {
            SetTeam(Team.Enemy);
            return Team.Enemy;
        }
        else
        {
            SetTeam(Team.Player);
            return Team.Player;
        }
    }

    private void ChangeAnimator()
    {
        if (Team == Team.Player)
        {
            _unitAnimator.runtimeAnimatorController = Data.CorruptionAnimatorController;
            if (Data.CorruptionSkillEffectAnim != null)
                SkillEffectAnim = Data.CorruptionSkillEffectAnim;
        }
        else
        {
            _unitAnimator.runtimeAnimatorController = Data.AnimatorController;
            if (Data.SkillEffectAnim != null)
                SkillEffectAnim = Data.SkillEffectAnim;
        }
    }
    
    public IEnumerator CutSceneMove(Vector3 moveVec, float moveTime)
    {
        if (moveCoro != null)
            StopCoroutine(moveCoro);

        Vector3 originVec = transform.position;
        float time = 0;

        while (time <= moveTime)
        {
            time += Time.deltaTime;
            float t = time / moveTime;

            transform.position = Vector3.Lerp(originVec, moveVec, t);
            yield return null;
        }
    }

    public void UnitMove(Vector2 coord)
    {
        bool backMove = ((_location - coord).x > 0) ^ GetFlipX() && ((_location - coord).x != 0);
        //왼쪽으로 가면 거짓, 오른쪽으로 가면 참
        //지금 왼쪽 보면 참, 지금 오른쪽 보면 거짓
        BattleManager.Instance.CheckStigma(DeckUnit, new Stigma_Assasination());
        if (moveCoro != null)
            StopCoroutine(moveCoro);

        moveCoro = MoveFieldPosition(coord, backMove);
        StartCoroutine(moveCoro);

        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.MOVE, this);
    }

    public IEnumerator MoveFieldPosition(Vector2 coord, bool backMove)
    {
        Vector3 dest = BattleManager.Field.GetTilePosition(coord);

        float moveTime = 0.2f;
        float backMoveTime = 0.2f;
        Vector3 overDistance = (dest - transform.position) * 0.03f;
        Vector3 pVel = Vector3.zero;
        Vector3 sVel = Vector3.zero;
        
        _location = coord;
        //GetModifiedScale check location.y
        float addScale = GetModifiedScale();

        if (backMove)
            SetFlipX(!GetFlipX());

        while (Vector3.Distance(dest + overDistance, transform.position) >= 0.03f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, dest + overDistance, ref pVel, moveTime);
            transform.localScale = Vector3.SmoothDamp(transform.localScale, new(addScale, addScale, 1), ref sVel, moveTime);
            yield return null;
        }

        pVel = Vector3.zero;

        while (Vector3.Distance(dest, transform.position) >= 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, dest, ref pVel, backMoveTime);

            yield return null;
        }

        SetLocation(coord);
    }

    //엑티브 타이밍에 대미지 바꿀 때용
    public int ChangedDamage = 0;

    public void Attack(BattleUnit unit, int damage)
    {
        if (unit != null)
        {
            ChangedDamage = damage;
            bool attackSkip = false;

            //공격 전 체크
            attackSkip |= BattleManager.Instance.ActiveTimingCheck(ActiveTiming.BEFORE_ATTACK, this, unit);

            //대미지 확정 시 체크
            attackSkip |= BattleManager.Instance.ActiveTimingCheck(ActiveTiming.DAMAGE_CONFIRM, this, unit);

            if (unit.FallEvent)
            {
                //타락시켰을 시 체크
                BattleManager.Instance.ActiveTimingCheck(ActiveTiming.FALL, this, unit);
                BattleManager.Instance.ActiveTimingCheck(ActiveTiming.UNIT_TERMINATE, this, unit);

                attackSkip = true;
            }

            if (attackSkip)
                return;

            unit.GetAttack(-ChangedDamage, this);

            //공격 후 체크
            BattleManager.Instance.ActiveTimingCheck(ActiveTiming.AFTER_ATTACK, this, unit);

            if (unit.GetHP() <= 0)
            {
                BattleManager.Instance.ActiveTimingCheck(ActiveTiming.UNIT_KILL, this, unit);
                BattleManager.Instance.ActiveTimingCheck(ActiveTiming.UNIT_TERMINATE, this, unit);
            }

            ChangedDamage = 0;
        }
    }

    public virtual void GetAttack(int value, BattleUnit caster)
    {
        //피격 전 체크
        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.BEFORE_ATTACKED, this, caster))
        {
            return;
        }
        _floatingDamage.Init(value);

        ChangeHP(value);

        //피격 후 체크
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.AFTER_ATTACKED, this, caster);
    }

    public virtual void GetHeal(int value, BattleUnit caster)
    {
        _floatingDamage.Init(value);
        ChangeHP(value);
    }

    public void ChangeHP(int value)
    {
        if (HP.GetCurrentHP() + value > BattleUnitTotalStat.MaxHP)
            value = BattleUnitTotalStat.MaxHP - HP.GetCurrentHP();

        DeckUnit.DeckUnitChangedStat.CurrentHP += value;
        HP.ChangeHP(value);
        _hpBar.RefreshHPBar(HP.FillAmount());
    }

    public virtual int GetHP() => HP.GetCurrentHP();

    public void ChangeFall(int value)
    {
        Fall.ChangeFall(value);
        DeckUnit.DeckUnitUpgradeStat.FallCurrentCount += value;
        _hpBar.RefreshFallGauge(Fall.GetCurrentFallCount());
    }

    public virtual BattleUnit GetOriginalUnit() => this;

    public virtual void SetBuff(Buff buff)
    {
        Buff.SetBuff(buff, this);
        BattleUnitChangedStat = Buff.GetBuffedStat();
        _hpBar.AddBuff(buff);
    }

    public void DeleteBuff(BuffEnum buffEnum)
    {
        Buff.DeleteBuff(buffEnum);
        BattleUnitChangedStat = Buff.GetBuffedStat();
        _hpBar.DeleteBuff(buffEnum);
    }

    public void AddMoveRange(bool[] addRangeList)
    {
        for (int i = 0; i < _moveRangeList.Length; i++)
        {
            _moveRangeList[i] |= addRangeList[i];
        }
    }

    public void SetAttackRange(bool[] setRangeList)
    {
        Array.Copy(setRangeList, _attackRangeList, setRangeList.Length);
    }

    public CutSceneType GetCutSceneType() => CutSceneType.center; // Skill 없어져서 바꿨어요

    public List<Vector2> GetAttackRange()
    {
        List<Vector2> RangeList = new();
        if (NextAttackSkip)
        {
            RangeList.Add(new Vector2(0, 0));

            return RangeList;
        }

        int Acolumn = 11;
        int Arow = 5;

        for (int i = 0; i < _attackRangeList.Length; i++)
        {
            if (_attackRangeList[i])
            {
                int x = (i % Acolumn) - (Acolumn >> 1);
                int y = (i / Acolumn) - (Arow >> 1);

                Vector2 vec = new(x, y);

                RangeList.Add(vec);
            }
        }

        List<Vector2> UnitRangeList = new();
        foreach (Vector2 vec in RangeList)
        {
            UnitRangeList.Add(vec);
        }

        foreach (ConnectedUnit unit in ConnectedUnits)
        {
            foreach (Vector2 vec in RangeList)
            {
                UnitRangeList.Add(unit.Location - _location + vec);
            }
        }

        return UnitRangeList;
    }

    public List<Vector2> GetMoveRange()
    {
        List<Vector2> RangeList = new();
        if (NextMoveSkip)
        {
            RangeList.Add(new Vector2(0, 0));

            return RangeList;
        }

        int Mrow = 5;
        int Mcolumn = 5;

        for (int i = 0; i < _moveRangeList.Length; i++)
        {
            if (_moveRangeList[i])
            {
                int x = (i % Mcolumn) - (Mcolumn >> 1);
                int y = -((i / Mcolumn) - (Mrow >> 1));

                Vector2 vec = new(x, y);

                RangeList.Add(vec);
            }
        }

        List<Vector2> UnitRangeList = new();
        foreach (Vector2 vec in RangeList)
        {
            UnitRangeList.Add(vec);
        }

        foreach (ConnectedUnit unit in ConnectedUnits)
        {
            foreach (Vector2 vec in RangeList)
            {
                UnitRangeList.Add(unit.Location - _location  + vec);
            }
        }

        return UnitRangeList;
    }

    public List<Vector2> GetSplashRange(Vector2 target, Vector2 caster)
    {
        List<Vector2> SplashList = new();

        int Scolumn = 11;
        int Srow = 5;

        for (int i = 0; i < Data.SplashRange.Length; i++)
        {
            if (Data.SplashRange[i])
            {
                int x = (i % Scolumn) - (Scolumn >> 1);
                int y = (i / Scolumn) - (Srow >> 1);

                if ((target - caster).x > 0) SplashList.Add(new Vector2(x, y)); //오른쪽
                else if ((target - caster).x < 0) SplashList.Add(new Vector2(-x, y)); //왼쪽
                else if ((target - caster).y > 0) SplashList.Add(new Vector2(y, x)); //위쪽
                else if ((target - caster).y < 0) SplashList.Add(new Vector2(-y, x)); //아래쪽
            }
        }
        return SplashList;
    }
}
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

    public SpriteRenderer UnitRenderer;
    public Animator UnitAnimator;
    public AnimationClip SkillEffectAnim;

    //[SerializeField] public UnitAIController AI;
    [SerializeField] public UnitHP HP;
    [SerializeField] public UnitFall Fall;
    [SerializeField] public UnitBuff Buff;
    [SerializeField] public UnitAction Action;
    [SerializeField] public UI_HPBar _hpBar;
    
    [SerializeField] private GameObject _floatingDamagePrefab;

    [SerializeField] public List<Stigma> StigmaList => DeckUnit.GetStigma();

    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;
    public bool FallEvent = false;

    private float _scale;
    private float GetModifiedScale() => _scale/* + ((_scale * 0.01f) * (_location.y - 1))*/;

    private bool[] _moveRangeList;
    private bool[] _attackRangeList;
    private bool _isTeleportOn => Buff.CheckBuff(BuffEnum.SacredStep);

    private IEnumerator _moveCoro;

    public bool IsConnectedUnit;
    public List<ConnectedUnit> ConnectedUnits;

    public bool NextMoveSkip = false;
    public bool NextAttackSkip = false;
    public bool IsDoneMove = false;     // (한턴 기준) 이동을 수행한 유닛인가?
    public bool IsDoneAttack = false;   // (한턴 기준) 공격을 수행한 유닛인가?
    public int AttackUnitNum;

    public void Init(Team team)
    {
        UnitRenderer = GetComponent<SpriteRenderer>();
        UnitAnimator = GetComponent<Animator>();

        UnitRenderer.sprite = Data.Image;
        UnitRenderer.color = new Color(1, 1, 1, 1);

        HP.Init(BattleUnitTotalStat.MaxHP, BattleUnitTotalStat.CurrentHP);
        Fall.Init(BattleUnitTotalStat.FallCurrentCount, BattleUnitTotalStat.FallMaxCount);

        Action = BattleManager.Data.GetUnitAction(Data.UnitActionType);
        Action.Init();

        SetHPBar();
        _hpBar.RefreshHPBar(HP.FillAmount());
        _hpBar.RefreshFallBar(Fall.GetCurrentFallCount(), FallAnimMode.Off);

        _scale = transform.localScale.x;

        _moveRangeList = new bool[Data.MoveRange.Length];
        Array.Copy(Data.MoveRange, _moveRangeList, Data.MoveRange.Length);

        _attackRangeList = new bool[Data.AttackRange.Length];
        Array.Copy(Data.AttackRange, _attackRangeList, Data.AttackRange.Length);
        
        BattleManager.Data.BattleUnitList.Add(this);

        GameManager.Sound.Play("Summon/SummonSFX");
        SetTeam(team);
    }

    public void UnitSetting(Vector2 coord, bool isConnectedUnit = false)
    {
        BattleManager.Field.EnterTile(this, coord);
        SetLocation(coord);

        IsConnectedUnit = isConnectedUnit;
        ConnectedUnits = new();

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
            
            SetFlipX(_team == Team.Enemy);
            _hpBar.SetPosition(this);

            //소환 시 체크
            BattleManager.Instance.ActiveTimingCheck(ActiveTiming.STIGMA, this);
            BattleManager.Instance.ActiveTimingCheck(ActiveTiming.SUMMON, this);
            BattleManager.Instance.UnitSummonEvent(this);
        }
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
        if (UnitRenderer.flipX == flip || Data.IsFlipFixed)
            return;

        UnitRenderer.flipX = flip;

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

            Dictionary<BattleUnit, Vector2> flipLocationDict = new();

            for (int i = 0; i < size; i++)
            {
                if ((int)_location.x == minX + i)
                {
                    BattleManager.Field.ExitTile(_location);
                    flipLocationDict.Add(this, new(maxX - i, _location.y));
                }

                foreach (ConnectedUnit unit in ConnectedUnits)
                {
                    if ((int)unit.Location.x == minX + i)
                    {
                        BattleManager.Field.ExitTile(unit.Location);
                        flipLocationDict.Add(unit, new(maxX - i, unit.Location.y));
                    }
                }
            }

            foreach (ConnectedUnit unit in ConnectedUnits)
            {
                unit.SetLocation(flipLocationDict[unit]);
                BattleManager.Field.EnterTile(unit, flipLocationDict[unit]);
            }

            SetLocation(flipLocationDict[this]);
            BattleManager.Field.EnterTile(this, flipLocationDict[this]);
        }
    }

    public bool GetFlipX() => UnitRenderer.flipX;

    public void SetHPBar()
    {
        _hpBar.SetHPBar(_team);
        _hpBar.SetFallBar(DeckUnit);
        _hpBar.RefreshBuff();
    }

    public void RefreshHPBar()
    {
        _hpBar.RefreshHPBar(HP.FillAmount());
        _hpBar.RefreshFallBar(Fall.GetCurrentFallCount(), FallAnimMode.On);
        _hpBar.RefreshBuff();
    }

    public void ResetHPBarPosition()
    {
        _hpBar.SetPosition(this, true);
    }

    public void SetLocation(Vector2 coord)
    {
        _location = coord;

        float scale = GetModifiedScale();
        
        if (_moveCoro != null)
            StopCoroutine(_moveCoro);

        transform.position = BattleManager.Field.GetTilePosition(coord);
        transform.localScale = new(scale, scale, 1);
    }

    public void UnitDiedEvent(bool isDeathAvoidable = true)
    {
        //자신이 사망 시 체크
        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.BEFORE_UNIT_DEAD, this))
        {
            if (isDeathAvoidable)
                return;
        }

        if (FallEvent)
        {
            return;
        }

        BattleManager.Instance.UnitDeadEvent(this);
        if (IsConnectedUnit)
        {
            BattleUnit originalUnit = this.GetOriginalUnit();
            if (originalUnit != null)
                originalUnit.UnitDiedEvent(isDeathAvoidable);
        }

        foreach (ConnectedUnit unit in ConnectedUnits)
        {
            BattleManager.Instance.UnitDeadEvent(unit);
            BattleManager.Spawner.RestoreUnit(unit.gameObject);
        }

        StartCoroutine(UnitDeadEffect());
        GameManager.VisualEffect.StartUnitDeadEffect(transform.position, GetFlipX());
        GameManager.Sound.Play("Dead/DeadSFX");
    }

    private IEnumerator UnitDeadEffect()
    {
        Color c = UnitRenderer.color;

        while (c.a > 0)
        {
            c.a -= 0.01f;

            UnitRenderer.color = c;

            yield return null;
        }

        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.AFTER_UNIT_DEAD, this);
        BattleManager.Spawner.RestoreUnit(gameObject);
    }

    public void UnitFallEvent()
    {
        if (Data.Rarity == Rarity.Original)
        {
            UnitDiedEvent(false);
            return;
        }

        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.FALLED, this))
            return;

        // 스팀 업적: 타락
        if (Team == Team.Enemy)
        {
            switch (Data.ID)
            {
                case "투발카인": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_TUBALCAIN); break;
                case "라헬&레아": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_RAHELLEA); break;
                case "엘리우스": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_ELIEUS); break;
                case "야나": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_YANA); break;
                case "압바임": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_APPAIM); break;
                case "바누엘": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_PHANUEL); break;
                case "구원자": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_THESAVIOR); break;
                case "리비엘": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_LIBIEL); break;
                case "아라벨라": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_ARABELLA); break;
                case "욘": GameManager.Steam.IncreaseAchievement(SteamAchievementType.CORRUPT_YOHRN); break; 
            }
        }

        FallEvent = true;

        Buff.ClearBuffByCorruption();

        DeckUnit.UnitID = BattleManager.UnitIDManager.GetID();
        BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(false);
        Invoke(nameof(CreateCorruptEffect), 0.5f);
    }

    void CreateCorruptEffect()
    {
        GameManager.Sound.Play("UI/FallSFX/Fall");
        GameManager.VisualEffect.StartCorruptionEffect(this, transform.position);
    }

    public void Corrupted()
    {
        //타락 이벤트 종료
        FallEvent = false;
        BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(true);

        foreach (ConnectedUnit unit in ConnectedUnits)
        {
            unit.ChangeTeam();
        }

        BattleManager.Instance.UnitFallEvent(this);

        if (ChangeTeam() == Team.Enemy)
        {
            Fall.Editfy();
            SetBuff(new Buff_Edified());
        }
        
        DeckUnit.DeckUnitChangedStat.CurrentHP = 0;
        DeckUnit.DeckUnitUpgradeStat.FallCurrentCount = 0;
        if (_team.Equals(Team.Player) && DeckUnit.DeckUnitTotalStat.FallMaxCount >= 4)
            DeckUnit.DeckUnitUpgradeStat.FallMaxCount = 4 - DeckUnit.DeckUnitTotalStat.FallMaxCount; // 타락 4개 이상인 적이 타락됐을 경우 조정

        HP.Init(DeckUnit.DeckUnitTotalStat.MaxHP, DeckUnit.DeckUnitTotalStat.MaxHP);
        Fall.Init(BattleUnitTotalStat.FallCurrentCount, BattleUnitTotalStat.FallMaxCount);

        SetHPBar();
        _hpBar.RefreshHPBar(HP.FillAmount());
        _hpBar.RefreshFallBar(Fall.GetCurrentFallCount(), FallAnimMode.On);

        BattleManager.Data.BattleUnitOrderSorting();
        BattleManager.BattleUI.WaitingLineRefresh();
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.STIGMA, this);
    }

    //애니메이션에서 직접 실행시킴
    public void AnimAttack()
    {
        BattleManager.BattleCutScene.IsAttack = true;
    }

    public void AnimatorSetBool(string varName, bool boolean)
    {
        UnitAnimator.SetBool(varName, boolean);
    }

    public void AnimatorSetInteger(string varName, int integer)
    {
        UnitAnimator.SetInteger(varName, integer);
    }

    public bool AnimatorIsMotionEnd() => UnitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

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
            UnitAnimator.runtimeAnimatorController = Data.CorruptionAnimatorController;
            if (Data.CorruptionSkillEffectAnim != null)
                SkillEffectAnim = Data.CorruptionSkillEffectAnim;
        }
        else
        {
            UnitAnimator.runtimeAnimatorController = Data.AnimatorController;
            if (Data.SkillEffectAnim != null)
                SkillEffectAnim = Data.SkillEffectAnim;
        }
    }
    
    public IEnumerator CutSceneMove(Vector3 moveDestination, float moveTime)
    {
        if (_moveCoro != null)
            StopCoroutine(_moveCoro);

        Vector3 originVec = transform.position;
        float time = 0;

        while (time <= moveTime)
        {
            time += Time.deltaTime;
            float t = time / moveTime;

            transform.position = Vector3.Lerp(originVec, moveDestination, t);
            yield return null;
        }
    }

    public void UnitMove(Vector2 coord, float moveSpeed, bool isFilpFix)
    {
        bool flip = (((_location - coord).x > 0) ^ GetFlipX() && ((_location - coord).x != 0)) && !(isFilpFix || Data.IsFlipFixed);
        //왼쪽으로 가면 거짓, 오른쪽으로 가면 참
        //지금 왼쪽 보면 참, 지금 오른쪽 보면 거짓
        if (_moveCoro != null)
            StopCoroutine(_moveCoro);

        _moveCoro = MoveFieldPosition(BattleManager.Field.GetTilePosition(coord), coord, flip, moveSpeed);
        StartCoroutine(_moveCoro);
    }

    public IEnumerator MoveFieldPosition(Vector3 moveDestination, Vector2 coord, bool flip, float moveSpeed)
    {
        _location = coord;
        yield return MoveFieldPositionCoroutine(moveDestination, flip, moveSpeed);
        SetLocation(_location);
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.MOVE, this);
    }

    public IEnumerator MoveFieldPositionCoroutine(Vector3 moveDestination, bool flip, float moveSpeed)
    {
        //GetModifiedScale check location.y
        float addScale = GetModifiedScale();

        if (flip)
            SetFlipX(!GetFlipX());

        Vector3 pVel = Vector3.zero;
        Vector3 sVel = Vector3.zero;
        Vector3 overDistance = (moveDestination - transform.position) * 0.03f;

        float moveTime = 0.18f / moveSpeed;
        float backMoveTime = 0.2f / moveSpeed;

        while (Vector3.Distance(moveDestination + overDistance, transform.position) >= 0.03f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, moveDestination + overDistance, ref pVel, moveTime);
            transform.localScale = Vector3.SmoothDamp(transform.localScale, new(addScale, addScale, 1), ref sVel, moveTime);

            yield return null;
        }

        pVel = Vector3.zero;

        while (Vector3.Distance(moveDestination, transform.position) >= 0.05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, moveDestination, ref pVel, backMoveTime);

            yield return null;
        }
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

                attackSkip = true;
            }

            if (!attackSkip)
            {
                unit.GetAttack(-ChangedDamage, this);
            }

            //공격 후 체크
            BattleManager.Instance.ActiveTimingCheck(ActiveTiming.AFTER_ATTACK, this, unit);

            if (unit.GetHP() <= 0)
            {
                BattleManager.Instance.ActiveTimingCheck(ActiveTiming.UNIT_KILL, this, unit);
            }

            ChangedDamage = 0;
        }
    }

    public virtual void GetAttack(int value, BattleUnit caster)
    {
        //피격 전 체크
        ChangedDamage = value;
        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.BEFORE_ATTACKED, this, caster))
        {
            return;
        }

        DisplayFloatingDamage(ChangedDamage);
        ChangeHP(value);

        //피격 후 체크
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.AFTER_ATTACKED, this, caster);
    }

    public virtual void GetHeal(int value, BattleUnit caster)
    {
        DisplayFloatingDamage(value);
        ChangeHP(value);
    }

    public void DisplayFloatingDamage(int value)
    {
        GameObject.Instantiate(_floatingDamagePrefab, this.transform).GetComponent<UI_FloatingDamage>().Init(value, UnitRenderer.flipX);
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

    public void ChangeFall(int value, BattleUnit caster, FallAnimMode fallAnimMode = FallAnimMode.On, float fallAnimDelay = 0.75f)
    {
        if (FallEvent || Fall.IsEdified)
        {
            Debug.Log($"{Data.Name} is Edified.");
            return;
        }

        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.BEFORE_CHANGE_FALL, this, caster))
        {
            return;
        }

        if (value < 0 && Fall.GetCurrentFallCount() + value < 0)
            value = -Fall.GetCurrentFallCount(); // 음수 방지

        if (value > 0 && Fall.GetCurrentFallCount() + value >= Fall.GetMaxFallCount())
            value = Fall.GetMaxFallCount() - Fall.GetCurrentFallCount(); // 최대치 방지

        Fall.ChangeFall(value);
        DeckUnit.DeckUnitUpgradeStat.FallCurrentCount += value;
        _hpBar.RefreshFallBar(Fall.GetCurrentFallCount(), fallAnimMode, fallAnimDelay);
    }

    public virtual BattleUnit GetOriginalUnit() => this;

    public virtual void SetBuff(Buff buff)
    {
        buff.Init(this);

        if (BattleManager.Instance.ActiveTimingCheck(ActiveTiming.BEFORE_BUFFED, this, null))
        {
            foreach (Buff activeBuff in Buff.CheckActiveTiming(ActiveTiming.BEFORE_BUFFED))
            {
                if (activeBuff.Active(buff))
                    return;
            }
        }

        Buff.SetBuff(buff);
        BattleUnitChangedStat = Buff.GetBuffedStat();
        _hpBar.AddBuff(buff);
        _hpBar.RefreshBuff();

        HP.Init(BattleUnitTotalStat.MaxHP, BattleUnitTotalStat.CurrentHP);
    }

    public void DeleteBuff(BuffEnum buffEnum)
    {
        Buff.DeleteBuff(buffEnum);
        BattleUnitChangedStat = Buff.GetBuffedStat();
        _hpBar.DeleteBuff(buffEnum);
        _hpBar.RefreshBuff();
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
        List<Vector2> UnitRangeList = new();

        if (NextMoveSkip)
        {
            UnitRangeList.Add(new Vector2(0, 0));
            return UnitRangeList;
        }
        else if (_isTeleportOn)
        {
            UnitRangeList = BattleManager.Field.GetUnitAllCoord();
            return UnitRangeList;
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

        foreach (Vector2 vec in RangeList)
        {
            UnitRangeList.Add(vec);
        }

        foreach (ConnectedUnit unit in ConnectedUnits)
        {
            foreach (Vector2 vec in RangeList)
            {
                if (!UnitRangeList.Contains(unit.Location - _location + vec))
                    UnitRangeList.Add(unit.Location - _location  + vec);
            }
        }

        return UnitRangeList;
    }

    public List<Vector2> GetSplashRange(Vector2 target, Vector2 caster)
    {
        List<Vector2> SplashList = new();
        SplashList.Add(Vector2.zero);

        int Scolumn = 11;
        int Srow = 5;

        for (int i = 0; i < Data.SplashRange.Length; i++)
        {
            if (Data.SplashRange[i])
            {
                int x = (i % Scolumn) - (Scolumn >> 1);
                int y = (i / Scolumn) - (Srow >> 1);

                if (x == 0 && y == 0)
                    continue;

                if ((target - caster).x > 0) SplashList.Add(new Vector2(x, y)); //오른쪽
                else if ((target - caster).x < 0) SplashList.Add(new Vector2(-x, y)); //왼쪽
                else if ((target - caster).y > 0) SplashList.Add(new Vector2(y, x)); //위쪽
                else if ((target - caster).y < 0) SplashList.Add(new Vector2(-y, x)); //아래쪽
            }
        }
        return SplashList;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public DeckUnit DeckUnit;
    public Stat Stat => DeckUnit.Stat + BattleUnitChangedStat;
    public UnitDataSO Data => DeckUnit.Data;

    [SerializeField] public Stat BattleUnitChangedStat; 

    [SerializeField] private Team _team;
    public Team Team => _team;

    private SpriteRenderer _renderer;
    private Animator UnitAnimator;
    public AnimationClip SkillEffectAnim;

    [SerializeField] public UnitAIController AI;

    [SerializeField] public UnitHP HP;
    [SerializeField] public UnitFall Fall;
    [SerializeField] public UnitSkill Skill;
    [SerializeField] public List<Passive> Passive => DeckUnit.Stigma;
    [SerializeField] private UI_HPBar _hpBar;

    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;

    private float scale;
    private float GetModifiedScale() => scale + ((scale * 0.1f) * (Location.y - 1));

    // 23.02.16 임시 수정
    private Action<BattleUnit> _UnitDeadAction;
    public Action<BattleUnit> UnitDeadAction
    {
        set { _UnitDeadAction = value; }
    }
    
    public void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();
        UnitAnimator = GetComponent<Animator>();

        AI.SetCaster(this);
        HP.Init(Stat.HP, Stat.CurrentHP);
        Fall.Init(Stat.FallCurrentCount, Stat.FallMaxCount);
        scale = transform.localScale.x;

        _renderer.sprite = Data.Image;
        GameManager.Sound.Play("Summon/SummonSFX");
    }

    public void SetHPBar()
    {
        _hpBar.SetHPBar(Team, transform);
        _hpBar.SetFallBar(DeckUnit);

    }

    public void SetTeam(Team team)
    {
        _team = team;

        // 적군일 경우 x축 뒤집기
        _renderer.flipX = (Team == Team.Enemy) ? true : false;
        SetHPBar();
        ChangeAnimator();
    }

    public void SetLocate(Vector2 coord) {
        _location = coord;
    }
    
    public void UnitDiedEvent()
    {
        _UnitDeadAction(this);
        if (_team == Team.Enemy)
        {
            GameManager.Data.DarkEssenseChage(Data.DarkEssenseDrop);
        }
        GameManager.Sound.Play("Dead/DeadSFX");
    }

    public void UnitFallEvent()
    {
        _hpBar.RefreshFallGauge(0);

        HP.Init(Stat.HP, Stat.CurrentHP);
        _hpBar.SetHPBar(Team, transform);
        BattleManager.Data.CorruptUnits.Add(this);

        GameManager.Sound.Play("UI/FallSFX/Fall");
        GameManager.VisualEffect.StartCorruptionEffect(this, transform.position);
    }

    public void Corrupted()
    {
        //타락 시 낙인 체크
        BattleManager.Data.CorruptUnits.Remove(this);

        if (ChangeTeam() == Team.Enemy)
        {
            Fall.Editfy();
        }
    }

    public void AnimAttack()
    {
        StartCoroutine(BattleManager.Instance.UnitAttack());
    }

    public Team ChangeTeam(Team team = default)
    {
        if(team != default)
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
        if(Team == Team.Player)
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

    public void SetPosition(Vector3 dest)
    {
        float s = GetModifiedScale();

        transform.position = dest;
        transform.localScale = new Vector3(s, s, 1);
    }

    public IEnumerator MovePosition(Vector3 dest)
    {
        float moveTime = 0.4f;
        float time = 0;
        Vector3 curP = transform.position;
        Vector3 curS = transform.localScale;

        float addScale = GetModifiedScale();

        while (time < moveTime)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(curP, dest, time / moveTime);
            transform.localScale = Vector3.Lerp(curS, new Vector3(addScale, addScale, 1), time / moveTime);
            yield return null;
        }
    }

    public void SkillUse(BattleUnit _unit) {
        if(_unit != null)
        {
            //피격 전 낙인 체크
            Skill.Use(this, _unit);
            //피격 후 낙인 체크
        }
    }                   

    public Stat GetStat(bool buff = true) {
        return Stat;
    }

    public void ChangeHP(int value) {
        HP.ChangeHP(value);
        _hpBar.RefreshHPBar(HP.FillAmount());
    }

    public void ChangeFall(int value)
    {
        Fall.ChangeFall(value);
        _hpBar.RefreshFallGauge(Fall.GetCurrentFallCount());
    }
    
    public bool GetFlipX() => _renderer.flipX;

    public CutSceneType GetCutSceneType() => CutSceneType.center; // Skill 없어져서 바꿨어요

    public List<Vector2> GetAttackRange()
    {
        List<Vector2> RangeList = new List<Vector2>();

        int Acolumn = 11;
        int Arow = 5;

        for (int i = 0; i < Data.AttackRange.Length; i++)
        {
            if (Data.AttackRange[i])
            {
                int x = (i % Acolumn) - (Acolumn >> 1);
                int y = (i / Acolumn) - (Arow >> 1);

                Vector2 vec = new Vector2(x, y);

                RangeList.Add(vec);
            }
        }

        return RangeList;
    }

    public List<Vector2> GetMoveRange()
    {
        List<Vector2> RangeList = new List<Vector2>();

        int Mrow = 5;
        int Mcolumn = 5;

        for (int i = 0; i < Data.MoveRange.Length; i++)
        {
            if (Data.MoveRange[i])
            {
                int x = (i % Mcolumn) - (Mcolumn >> 1);
                int y = -((i / Mcolumn) - (Mrow >> 1));

                Vector2 vec = new Vector2(x, y);

                RangeList.Add(vec);
            }
        }

        return RangeList;
    }

    public IEnumerator ShakeUnit(float shakeCount, float shakeTime, float shakePower)
    {
        Vector3 originPos = transform.position;

        float count = shakeCount;
        float time = shakeTime / shakePower;
        bool min = true;
        transform.position += new Vector3(shakePower / 2, 0, 0);

        while (count > 0)
        {
            yield return new WaitForSeconds(time / shakeCount);

            if (gameObject == null)
                yield break;

            Vector3 vec = new Vector3(shakePower / shakeCount * count, 0, 0);
            if (min) vec *= -1;
            transform.position += vec;
            count--;
            min = !min;
        }

        transform.position = originPos;
    }

    public List<Vector2> GetSplashRange(Vector2 target, Vector2 caster)
    {
        List<Vector2> SplashList = new List<Vector2>();

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


    // 낙인 타입에 따라 낙인 내용 실행하는 함수 BattleManager나 BattleUnit 혹은 제 3자에 넣을 지 고민 중
    public void PassiveCheck(BattleUnit caster, BattleUnit receiver, PassiveType type)
    {
        if(type == PassiveType.BEFOREATTACKED || type == PassiveType.AFTERATTACKED || type == PassiveType.FALLED)
        {
            foreach (Passive passive in receiver.Passive)
            {
                if (passive.PassiveType == type)
                {
                    passive.Use(caster, receiver);
                }
            }
        }
        else
        {
            foreach(Passive passive in Passive)
            {
                if (passive.PassiveType == type)
                {
                    passive.Use(caster, receiver);
                }
            }
        }   
    }

    public void OnDestroy()
    {
        DeckUnit.ChangedStat.CurrentHP = HP.GetCurrentHP() - DeckUnit.Stat.HP;
        DeckUnit.ChangedStat.FallCurrentCount = Fall.GetCurrentFallCount();
    }
}

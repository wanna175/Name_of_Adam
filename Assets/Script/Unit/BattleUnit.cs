using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : DeckUnit
{
    [SerializeField] private Team _team;
    public Team Team => _team;

    private SpriteRenderer _renderer;
    private Animator _animator;

    public Unit_AI_Controller AI;

    [SerializeField] public UnitHP HP;
    [SerializeField] public UnitFall Fall;
    [SerializeField] public UnitSkill Skill;

    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;

    // 23.02.16 임시 수정
    private Action<BattleUnit> _UnitDeadAction;
    public Action<BattleUnit> UnitDeadAction
    {
        set { _UnitDeadAction = value; }
    }
    
    public void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        AI.SetCaster(this);
        HP.Init(Stat.HP, Stat.CurrentHP);
        Fall.Init(Stat.Fall);

        _renderer.sprite = Data.Image;
    }

    public void SetTeam(Team team)
    {
        _team = team;

        // 적군일 경우 x축 뒤집기
        _renderer.flipX = (Team == Team.Enemy) ? true : false;
    }

    public void SetLocate(Vector2 coord) {
        _location = coord;
    }
    
    public void UnitDiedEvent()
    {
        _UnitDeadAction(this);
        Destroy(gameObject);
    }

    public void UnitFallEvent()
    {
        ChangeTeam();
        ChangedStat.CurrentHP = Stat.HP;
        HP.Init(Stat.HP, Stat.CurrentHP);
        Debug.Log($"{Data.name} Fall");
    }

    public void ChangeTeam(Team team = default)
    {
        if(team != default)
        {
            SetTeam(team);
            return;
        }
        
        if (Team == Team.Player)
            SetTeam(Team.Enemy);
        else
            SetTeam(Team.Player);
    }

    public void SetPosition(Vector3 dest)
    {
        transform.position = dest;
    }
    

    public void SkillUse(BattleUnit _unit) {
        if(_unit != null)
        {
            Skill.Use(this, _unit);
        }
    }                   

    public Stat GetStat(bool buff = true) {
        return Stat;
    }

    public void ChangeHP(int value) {
        HP.ChangeHP(value);
    }

    public void ChangeFall(int value)
    {
        Fall.ChangeFall(value);
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

    public List<Vector2> GetSplashRange(Vector2 target)
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

                if ((target - Location).x > 0) //오른쪽
                {
                    SplashList.Add(new Vector2(x, y));
                }
                else if ((target - Location).x < 0) //왼쪽
                {
                    SplashList.Add(new Vector2(-x, y));
                }
                else if ((target - Location).y > 0) //위쪽
                {
                    SplashList.Add(new Vector2(y, x));
                }
                else if ((target - Location).y < 0) //아래쪽
                {
                    SplashList.Add(new Vector2(-y, x));
                }
            }
        }
        return SplashList;
    }
}

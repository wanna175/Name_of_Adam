using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleUnit : Unit
{
    [SerializeField] private Team _team;
    public Team Team => _team;

    private SpriteRenderer _renderer;
    private Animator _animator;

    public Unit_AI_Controller AI;

    [SerializeField] public UnitHP HP;
    [SerializeField] public UnitFall Fall;
    

    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;

    // 23.02.16 임시 수정
    private Action<BattleUnit> _UnitDeadAction;
    public Action<BattleUnit> UnitDeadAction
    {
        set { _UnitDeadAction = value; }
    }
    
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        AI.SetCaster(this);
        HP.Init(Stat.HP);
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
        Debug.Log($"{Data.name} Fall");
    }

    public void ChangeTeam(Team team = default)
    {
        if(team != default)
        {
            _team = team;
            return;
        }
        
        if (Team == Team.Player)
            _team = Team.Enemy;
        else
            _team = Team.Player;
    }

    public void SetPosition(Vector3 dest)
    {
        transform.position = dest;
    }
    

    public void SkillUse(BattleUnit _unit) {
        if(_unit != null)
        {
            //Data.Skill.Use(this, _unit);
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
    
    public List<Vector2> GetAttackRange() => Data.GetAttackRange();
    
    public List<Vector2> GetMoveRange() => Data.GetMoveRange();
}
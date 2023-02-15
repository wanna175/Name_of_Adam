using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleUnit : Unit
{
    [SerializeField] private Team _team;
    public Team Team => _team;

    [SerializeField] private int _fallGauge;
    public int FallGauge => _fallGauge;
    private Skill _skill;

    BattleManager _BattleMNG;
    BattleDataManager _BattleDataMNG;
    CutSceneManager _CutSceneMNG;

    private SpriteRenderer _renderer;
    private Animator _animator;

    [SerializeField] public UnitHP HP;
    
    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;

    public Vector2 _SelectTile = new Vector2(-1, -1);
    public Vector2 SelectTile => _SelectTile;
    
    private void Awake()
    {
        _BattleMNG = GameManager.Battle;
        _BattleDataMNG = GameManager.Battle.Data;
        _CutSceneMNG = GameManager.CutScene;

        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _skill = GetComponent<Skill>();

        _renderer.sprite = Data.Image;
    }

    public void Init(Team team, Vector2 coord)
    {
        _BattleDataMNG.BattleUnitAdd(this);
        HP.Init(Stat.HP);
        _team = team;

        // 적군일 경우 x축 뒤집기
        _renderer.flipX = (Team == Team.Enemy) ? true : false;
        setLocate(coord);
    }

    public void OnAttack(List<BattleUnit> _HitUnits)
    {
        //_CutSceneMNG.BattleCutScene(this, _HitUnits);
        foreach (BattleUnit unit in _HitUnits)
            unit.Hit_GetDamage(GetStat().ATK);

        _BattleMNG.UseNextUnit();
    }
    
    public void Hit_GetDamage(int DMG)
    {
        HP.ChangeHP(-DMG);
    }
    
    //오브젝트 생성 시, 최초 위치 설정
    public void setLocate(Vector2 coord)
    {
        _location = coord;
        _BattleMNG.SetUnit(this, coord);
    }
    
    public void UnitDiedEvent()
    {
        _BattleDataMNG.BattleUnitRemove(this);
        _BattleMNG.BattleOrderRemove(this);
        Destroy(gameObject);
    }

    public void SetPosition(Vector3 dest)
    {
        transform.position = dest;
    }
    

    public void AttackTileClick(BattleUnit _unit)
    {

        _skill.Use(this, _unit);
        _BattleMNG.UseNextUnit();
    }

    public Stat GetStat(bool buff = true)
    {
        return Stat;
    }

    public bool GetFlipX() => _renderer.flipX;
    
    public void ChangeHP(int value)
    {
        HP.ChangeHP(value);
    }

    public CutSceneType GetCutSceneType() => CutSceneType.center; // Skill 없어져서 바꿨어요
    
    public List<Vector2> GetAttackRange() => Data.GetAttackRange();
    
    public List<Vector2> GetMoveRange() => Data.GetMoveRange();
}
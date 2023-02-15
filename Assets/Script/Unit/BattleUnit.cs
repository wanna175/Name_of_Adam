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

    public void MoveTileClick(Vector2 coord)
    {
        coord -= Location;

        // 이동범위 밖을 선택했다면 다시 선택하기
        if (!GetMoveRange().Contains(coord))
        {
            _BattleMNG.SetTileColor(Color.yellow);
            return;
        }
        
        _BattleMNG.MoveLotate(this, coord);
        
        _BattleMNG.ChangeClickType();
        return;
    }

    public void AttackTileClick(Vector2 coord)
    {
        Vector2 dump = coord - Location;

        // 공격범위 밖을 선택했으면 다시 선택하기
        if (!GetAttackRange().Contains(dump))
        {
            _BattleMNG.SetTileColor(Color.yellow);
            return;
        }

        _SelectTile = coord;

        if (_SelectTile == Location)
            _BattleMNG.UseNextUnit();
        else
            _skill.Use(this, this); // 수정 필요
        
        _BattleMNG.ChangeClickType();
        return;
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
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
    private int _moveDistance;
    private Skill Skill; // Memo : 임시

    [SerializeField] SkillSO skill;

    BattleManager _BattleMNG;
    BattleDataManager _BattleDataMNG;
    CutSceneManager _CutSceneMNG;

    private SpriteRenderer _renderer;
    private Animator _animator;

    [SerializeField] public UnitHP HP;
    

    #region Location
    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;
    #endregion

    public Vector2 _SelectTile = new Vector2(-1, -1);
    public Vector2 SelectTile => _SelectTile;

    // Move인지 Attack인지 확인하기 위한 임시클래스
    bool isMove = true;
    public bool IsMove => isMove;
    
    private void Awake()
    {
        _BattleMNG = GameManager.Battle;
        _BattleDataMNG = GameManager.Battle.Data;
        _CutSceneMNG = GameManager.CutScene;

        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
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

    // 이동가능한 범위를 가져온다.
    public List<Vector2> GetCanMoveRange()
    {
        List<Vector2> vecList = new List<Vector2>();
        vecList.Add(new Vector2(0, 0));
        
        for (int i = 1; i <= _moveDistance; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                vecList.Add(new Vector2(i * j, 0));
                vecList.Add(new Vector2(0, i * j));
            }
        }

        return vecList;
    }


    public void Attack_OnAttack(List<BattleUnit> _HitUnits)
    {
        _CutSceneMNG.BattleCutScene(this, _HitUnits);
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


    public void TileSelected(Vector2 coord)
    {
        if (isMove)
            MoveTileClick(coord);
        else
            AttackTileClick(coord);
    }

    void MoveTileClick(Vector2 coord)
    {
        coord -= Location;

        // 이동범위 밖을 선택했다면 다시 선택하기
        if (!GetCanMoveRange().Contains(coord))
        {
            _BattleMNG.SetTileColor(Color.yellow);
            return;
        }

        _BattleMNG.MoveLotate(this, coord);
        //ChangeState(BattleUnitState.AttackWait);
        //UpdateState();

        isMove = false;
        return;
    }

    public void AttackTileClick(Vector2 coord)
    {
        Vector2 dump = coord - Location;

        // 공격범위 밖을 선택했으면 다시 선택하기
        if (!GetRange().Contains(dump))
        {
            _BattleMNG.SetTileColor(Color.yellow);
            return;
        }

        _SelectTile = coord;

        if (_SelectTile == Location)
            _BattleMNG.UseNextUnit();
        else
            use(this);

        isMove = true;
        return;
    }

    public Stat GetStat(bool buff = true)
    {
        return Stat;
    }

    public bool GetFlipX() => _renderer.flipX;

    public void use(BattleUnit ch)
    {
        skill.use(ch);
    }

    public CutSceneType GetCutSceneType() => skill.CSType;

    public List<Vector2> GetRange() => skill.GetRange();

    public int SkillLength() => skill.EffectList.Count;
}
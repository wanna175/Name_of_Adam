using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitAction : MonoBehaviour
{
    BattleUnit _BattleUnit;
    BattleUnitSO BattleUnitSO;
    BattleDataManager _BattleDataMNG;

    #region HP
    [SerializeField] float _MaxHP, _CurHP;
    public float MaxHP => _MaxHP;
    public float CurHP
    {
        get { return _CurHP; }
        set
        {
            _CurHP += value;

            if (MaxHP < _CurHP)
                _CurHP = MaxHP;
            else if (_CurHP < 0)
                _CurHP = 0;
        }
    }
    #endregion

    private void Awake()
    {
        _BattleUnit = GetComponent<BattleUnit>();
    }

    private void Start()
    {
        _BattleDataMNG = GameManager.Instance.BattleMNG.BattleDataMNG;
    }


    public void GetMaxHP(float _hp)
    {
        _MaxHP = _CurHP = _hp;
    }

    #region Hit & Destroy

    // 데미지를 받을 때
    public void GetDamage(float DMG)
    {
        CurHP -= DMG;

        Debug.Log("DMG : " + DMG + ", CurHP ; " + CurHP);

        if (MaxHP <= CurHP)
            CurHP = MaxHP;

        DeadCheck();
    }

    // 캐릭터가 사망했는지 확인
    void DeadCheck()
    {
        if (CurHP <= 0)
        {
            // 죽었을 때 처리할 것들
            _BattleDataMNG.BattleUnitMNG.BattleUnitExit(_BattleUnit);
            _BattleDataMNG.FieldMNG.ExitTile(_BattleUnit.UnitMove.LocX, _BattleUnit.UnitMove.LocY);

            Destroy(gameObject);
        }
    }

    #endregion


    // 타락 게이지가 늘어나거나 줄어들 때
    public void SetFallGauge(int value)
    {
        int gauge = BattleUnitSO.FallGauge;
        int maxGauge = 3;

        gauge += value;
        if (gauge < 0) gauge = 0;
        else if (gauge >= maxGauge)
        {
            Fall();
            gauge = 0;
        }

        _BattleUnit.BattleUnitSO.FallGauge = gauge;
    }

    void Fall()
    {
        BattleUnitSO.Fall = true;
        if (BattleUnitSO.team == Team.Enemy)
            BattleUnitSO.team = Team.Player;
        else
            BattleUnitSO.team = Team.Enemy;
    }
}

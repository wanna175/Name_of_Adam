using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePrepareManager : MonoBehaviour
{
    BattleDataManager _BattleDataMNG;

    private void Start()
    {
        _BattleDataMNG = GameManager.Instance.BattleMNG.BattleDataMNG;
    }

    public void PrepareStart()
    {
        //UI 튀어나옴
        //GameManager.Instance.InputMNG.Hands.comebackHands();
        //UI가 작동할 수 있게 해줌
    }

    public void PrepareEnd()
    {
        //UI 들어감
        //GameManager.Instance.InputMNG.Hands.begoneHands();
        //UI 사용 불가
    }

    private BattleUnit _SelectedUnit;
    public BattleUnit SelectedUnit
    {
        get { return _SelectedUnit; }
        set { _SelectedUnit = value; }
    }

}

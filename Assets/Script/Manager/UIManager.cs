using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //UI들의 상호작용 관련 변수 관리
    [SerializeField] public Hands Hands;
    [SerializeField] public WatingLine WatingLine;

    private BattleUnit _SelectedUnit;
    public BattleUnit SelectedUnit
    {
        get { return _SelectedUnit; }
        set { _SelectedUnit = value; }
    }
}
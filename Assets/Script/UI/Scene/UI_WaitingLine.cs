using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingLine : UI_Scene
{
    [SerializeField] GameObject P_WaitingUnit;
    private List<UI_WaitingUnit> _waitingUnitList = new List<UI_WaitingUnit>();
    private Transform _grid;

    #region BattleUnitList  
    List<BattleUnit> _BattleUnitOrderList;
    #endregion

    private BattleManager _BattleMNG;

    public void Start()
    {
        _grid = Util.FindChild(gameObject, "Grid").transform;
        //StartCoroutine(Test());
        _BattleMNG = GameManager.Battle;
    }

    IEnumerator Test()
    {
        for(int i=0; i<6; i++)
        {
            UI_WaitingUnit newUnit = GameObject.Instantiate(P_WaitingUnit, _grid).GetComponent<UI_WaitingUnit>();
            _waitingUnitList.Add(newUnit);
            yield return new WaitForSeconds(1f);
        }
    }

    public void Generate_WaitingUnit(int i)
    {
        _waitingUnitList[i].enabled = true;
    }

    public void SetBattleOrderList()
    {
        _BattleUnitOrderList = _BattleMNG.GetUnitbyOrder();
    }

    public void SetWaitingLine()
    {
        for (int i = 0; i < _BattleUnitOrderList.Count; i++)
        {
            GameObject obj = Instantiate(P_WaitingUnit, transform);
            _waitingUnitList.Add(obj.GetComponent<UI_WaitingUnit>());
            _waitingUnitList[i].SetUnit(_BattleUnitOrderList[i].Data.Image);
        }
    }
}

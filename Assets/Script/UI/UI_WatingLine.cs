using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WatingLine : MonoBehaviour
{
    [SerializeField] GameObject WatingLinePrefabs; 
    private List<UI_WatingUnit> _WatingUnitList;
    
    #region BattleUnitList  
    List<BattleUnit> _BattleUnitOrderList;
    #endregion

    public void Start()
    {
        _WatingUnitList = new List<UI_WatingUnit>();
        //임시입니다. 진짜 임시입니다 

        for (int i = 0; i < 8; i++)
        {
            GameObject obj = Instantiate(WatingLinePrefabs, transform);
            obj.transform.position = new Vector3(-4.4f + 1.2f * i, 5);
            _WatingUnitList.Add(obj.GetComponent<UI_WatingUnit>());
        }
    }

    public void SetBattleUnitList(List<BattleUnit> list)
    {
        _BattleUnitOrderList = list;
    }

    public void SetWatingLine()
    {
        for (int i = 0; i < 8; i++)
        {   
            if (i < _BattleUnitOrderList.Count)
            {
                Debug.Log("WatingLine 1");
                _WatingUnitList[i].SetUnit(_BattleUnitOrderList[i].BattleUnitSO.sprite);
            }
            else
            {
                Debug.Log("WatingLine 2");
                _WatingUnitList[i].RemoveUnit();
            }
            
        }
    }
}

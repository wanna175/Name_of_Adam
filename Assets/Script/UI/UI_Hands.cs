using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hands : MonoBehaviour
{
    [SerializeField] GameObject HandPrefabs;
    private List<UI_Hand> _HandList;
    
    BattleDataManager _BattleDataMNG;

    void Start()
    {
        _BattleDataMNG = GameManager.Battle.BattleDataMNG;

        _HandList = new List<UI_Hand>();

        for (int i = 0; i < 4; i++)
        {
            GameObject obj = Instantiate(HandPrefabs, transform);
            obj.transform.position = new Vector3(3 + 2 * i, -5.3f);

            _HandList.Add(obj.GetComponent<UI_Hand>());
        }
    }

    void OnMouseDown()
    {
        Debug.Log("?");
        //전투 시작 후 초기 멀리건 4장
        for (int i = 0; i < 4; i++) {
            AddUnitToHand();
        }
    }

    public void AddUnitToHand()
    {
        //1,2,3,4 순으로 Hand의 Unit이 null이면 1개 추가
        foreach (UI_Hand h in _HandList)
        {
            if (h.IsHandNull())
            {
                h.SetHandDeckUnit(_BattleDataMNG.GetRandomDeckUnit());
                break;
            }
        }
    }
    public DeckUnit RemoveHand(int handIndex)
    {
        //handIndex는 1부터 시작하기에 -1 해야함
        DeckUnit returnUnit;

        returnUnit = _HandList[handIndex].RemoveHandDeckUnit();

        for (int i = handIndex+1; i < 4; i++)
        {
            _HandList[i-1].SetHandDeckUnit(_HandList[i].RemoveHandDeckUnit());
        }

        //빈 공간이 있으면 1개 추가
        AddUnitToHand();

        return returnUnit;
    }

    public void ReturnHand()
    {
        foreach (UI_Hand h in _HandList)
        {
            if (!h.IsHandNull())
            {
                _BattleDataMNG.AddDeckUnit(h.RemoveHandDeckUnit());
            }
        }

        _HandList = null;
    }

    #region Hand Click
    private int _ClickedHand = 0;
    public int ClickedHand => _ClickedHand;

    private DeckUnit _ClickedUnit = null;
    public DeckUnit ClickedUnit => _ClickedUnit;

    public void OnHandClick(UI_Hand hand)
    {
        _ClickedHand = _HandList.IndexOf(hand);
        _ClickedUnit = hand.GetHandDeckUnit();

        if (!_BattleDataMNG.CanUseMana(_ClickedUnit.GetUnitSO().ManaCost)){
            Debug.Log("not enough mana");
            ClearHand();
        }
    }

    public void ClearHand()
    {
        _ClickedHand = 0;
        _ClickedUnit = null;
    }
    #endregion
}

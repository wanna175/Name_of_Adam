using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] GameObject TilePrefabs;

    BattleManager _BattleMNG;
    BattleDataManager _BattleDataMNG;
    FieldManager _FieldMNG;
    InputManager _InputMNG;

    private void Awake()
    {
        _BattleMNG = GameManager.Instance.BattleMNG;
        _BattleDataMNG = _BattleMNG.BattleDataMNG;
        _FieldMNG = _BattleMNG.BattleDataMNG.FieldMNG;
        _InputMNG = GameManager.Instance.InputMNG;
        
        _FieldMNG.FieldSet(this, TilePrefabs);

        transform.position = _FieldMNG.FieldPosition;
        transform.eulerAngles = new Vector3(16, 0, 0);
    }

    public void TileClick(Tile tile)
    {
        List<List<Tile>> tiles = _FieldMNG.TileArray;
        int tileX = 0,
            tileY = 0;
        
        for(int i = 0; i < tiles.Count; i++)
        {
            for(int j = 0; j < tiles[i].Count; j++)
            {
                if(ReferenceEquals(tile, tiles[i][j]))
                {
                    tileX = j;
                    tileY = i;
                }
            }
        }

        // 현재 클릭 상태가 어떤 상태인지, 클릭 가능한지 체크하는 클래스 생성 필요

        // 유닛이 공격할 타겟을 선택중이라면
        if (tile.CanSelect)
        {
            _BattleMNG.PrepareMNG.SelectedUnit.TileSelected(tileX, tileY);
            _FieldMNG.FieldClear();
            return;
        }
        // 클릭한 타일에 유닛이 있을 시
        else if (tile.TileUnit != null)
        {
            BattleUnit SelectUnit = tile.TileUnit;
            _FieldMNG.FieldClear();
            
            // 그 유닛이 아군이라면
            if (tile.TileUnit.BattleUnitSO.team == Team.Player)
            {
                _BattleMNG.PrepareMNG.SelectedUnit = SelectUnit;

                // 유닛이 보유한 스킬이 타겟팅 형식인지 확인한다.
                List<Vector2> vecList = SelectUnit.BattleUnitSO.GetTargetingRange();
                if (vecList != null)
                {
                    // 타겟팅이 맞다면 범위 표시
                    for (int i = 0; i < vecList.Count; i++)
                    {
                        int x = SelectUnit.UnitMove.LocX - (int)vecList[i].x;
                        int y = SelectUnit.UnitMove.LocY - (int)vecList[i].y;

                        if (0 <= x && x < 8)
                        {
                            if (0 <= y && y < 3)
                                tiles[y][x].SetCanSelect(true);
                        }
                    }
                }
            }
        }
        // 클릭한 타일에 유닛이 없을 시
        else
        {
            // 당장은 핸드가 없어서 아래는 주석처리

            ////핸드를 누르고 타일을 누를 때
            //if (GameManager.Instance.InputMNG.ClickedHand != 0)
            //{
            //    //범위 외
            //    if (tileX > 3 && tileY > 2)
            //    {
            //        Debug.Log("out of range");
            //    }
            //    else
            //    {
            //        if (_BattleDataMNG.ManaMNG.UseMana(2))
            //        {
            //            //조건문이 참이라면 이미 마나가 소모됨
            //            _InputMNG.ClickedChar.UnitMove.setLocate(tileX, tileY);

            //            Instantiate(_InputMNG.ClickedChar);

            //            _InputMNG.DeleteHand(GameManager.Instance.InputMNG.ClickedHand);
            //            _InputMNG.ClearHand();

            //            _BattleDataMNG.BattleUnitMNG.BattleUnitList.Add(_InputMNG.ClickedChar);
            //        }
            //        else
            //        {
            //            //마나 부족
            //            Debug.Log("not enough mana");
            //        }
            //    }
            //}
        }
    }
}

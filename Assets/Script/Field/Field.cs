using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] GameObject TilePrefabs;

    FieldDataManager _FieldMNG;

    private void Awake()
    {
        GameManager.Instance.BattleMNG.BattleDataMNG.FieldDataMNG.FieldSet(transform, TilePrefabs);

        _FieldMNG = GameManager.Instance.BattleMNG.BattleDataMNG.FieldDataMNG;

        transform.position = GameManager.Instance.BattleMNG.BattleDataMNG.FieldDataMNG.FieldPosition;
        transform.eulerAngles = new Vector3(16, 0, 0);
    }

    public void TileClick(Tile tile)
    {
        List<List<Tile>> tiles = GameManager.Instance.BattleMNG.BattleDataMNG.FieldDataMNG.TileArray;
        int tileX, tileY;
        
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


        
        //핸드를 누르고 타일을 누를 때
        //    if (GameManager.Instance.InputMNG.ClickedHand != 0)
        //    {
        //        //범위 외
        //        if (LocX > 3 && LocY > 2)
        //        {
        //            Debug.Log("out of range");
        //        }
        //        else
        //        {
        //            if (GameManager.Instance.BattleMNG.UseMana(2))
        //            {
        //                //조건문이 참이라면 이미 마나가 소모됨
        //                GameManager.Instance.InputMNG.ClickedChar.setLocate(LocX, LocY);

        //                Instantiate(GameManager.Instance.InputMNG.ClickedChar);

        //                GameManager.Instance.InputMNG.DeleteHand(GameManager.Instance.InputMNG.ClickedHand);
        //                GameManager.Instance.InputMNG.ClearHand();

        //                GameManager.Instance.DataMNG.BattleCharList.Add(GameManager.Instance.InputMNG.ClickedChar);
        //            }
        //            else
        //            {
        //                //마나 부족
        //                Debug.Log("not enough mana");
        //            }
        //        }
        //    }

        //    if (CanSelect)
        //    {
        //        GameManager.Instance.BattleMNG.BattleDataMNG.FieldDataMNG.CanSelectClear();
        //        GameManager.Instance.BattleMNG.SelectedChar.TileSelected(LocY, LocX);
        //        return;
        //    }
        //    if (_TileUnit != null)
        //    {
        //        if (_TileUnit.characterSO.team == Team.Player)
        //        {
        //            GameManager.Instance.BattleMNG.SelectedChar = _TileUnit;

        //            List<Vector2> vecList = _TileUnit.characterSO.HaveTargeting();
        //            if (vecList != null)
        //            {
        //                GameManager.Instance.BattleMNG.BattleDataMNG.FieldDataMNG.CanSelectClear();
        //                Tile[,] tiles = GameManager.Instance.BattleMNG.BattleDataMNG.FieldDataMNG.TileArray;

        //                for (int i = 0; i < vecList.Count; i++)
        //                {
        //                    int x = _TileUnit.LocX - (int)vecList[i].x;
        //                    int y = _TileUnit.LocY - (int)vecList[i].y;

        //                    if (0 <= x && x < 8)
        //                    {
        //                        if (0 <= y && y < 3)
        //                        {
        //                            tiles[y, x].SetCanSelect(true);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}

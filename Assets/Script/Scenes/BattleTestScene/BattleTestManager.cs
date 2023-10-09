using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTestManager : BattleManager
{
    [SerializeField] public BattleTestDataContainer dataContainer;

    int nowClick = 0;

    public void AllUnitHeal()
    {
        foreach (BattleUnit unit in Data.BattleUnitList)
        {
            unit.ChangeHP(1000);
            unit.ChangeFall(-10);
        }
        Field.ClearAllColor();
    }

    public void SelectUnitDead()
    {
        Field.ClearAllColor();

        if (nowClick == 1)
        {
            nowClick = 0;
            return;
        }

        nowClick = 1;
        Field.SetUnitTileColor(FieldColorType.none);
    }

    public void SelectUnitFall()
    {
        Field.ClearAllColor();

        if (nowClick == 2)
        {
            nowClick = 0;
            return;
        }


        nowClick = 2;
        Field.SetUnitTileColor(FieldColorType.none);
    }

    public void CustomEffect()
    {
        Field.ClearAllColor();

        if (nowClick == 3)
        {
            nowClick = 0;
            return;
        }


        nowClick = 3;
        Field.SetAllTileColor(FieldColorType.none);
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (nowClick == 0)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hit;

            hit = Physics2D.GetRayIntersectionAll(ray);

            if (hit.Length != 0)
            {
                Vector2 coord = new Vector2(-1, -1);

                foreach (RaycastHit2D h in hit)
                {
                    if (h.transform.name == "Tile")
                        coord = Field.GetCoordByTile(h.transform.GetComponent<Tile>());
                }

                if (!Field.ColoredTile.Contains(coord))
                    return;

                TestClickEvent(coord);

                nowClick = 0;
                Field.ClearAllColor();
            }
        }
    }

    private void TestClickEvent(Vector2 coord)
    {
        if (nowClick == 1)
        {
            BattleUnit unit = Field.GetUnit(coord);
            while (unit.HP.GetCurrentHP() > 0)
                unit.ChangeHP(-1000);
        }
        else if (nowClick == 2)
        {
            BattleUnit unit = Field.GetUnit(coord);
            unit.ChangeFall(100);
        }
        else if (nowClick == 3)
        {
            GameManager.VisualEffect.StartVisualEffect(dataContainer.CustomEffect, Field.GetTilePosition(coord));
        }
    }
}
using UnityEngine;

public class Stigma_Advent : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        int count = 1;
        if (GameManager.OutGameData.IsUnlockedItem(74))
            count = 2;

        for (int i = 0; i < count; i++)
            caster.SetBuff(new Buff_Stigma_Advent());
    }
}
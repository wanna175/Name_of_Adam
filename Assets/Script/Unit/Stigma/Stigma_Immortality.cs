using UnityEngine;

public class Stigma_Immortality : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Immortality immortality = new();
        caster.SetBuff(immortality);
    }
}

using UnityEngine;

public class Stigma_GardenOfOblivion : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Karma());
    }
}

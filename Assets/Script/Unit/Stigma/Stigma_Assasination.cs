using UnityEngine;

public class Stigma_Assasination : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);
        // 이 성흔은 BattleUnit이 아닌, BattleManager에서 직접 수행됩니다.
    }
}

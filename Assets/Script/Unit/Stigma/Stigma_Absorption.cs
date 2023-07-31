public class Stigma_Absorption : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        double heal = caster.BattleUnitTotalStat.ATK * 0.3;
        caster.ChangeHP(((int)heal));
    }
}
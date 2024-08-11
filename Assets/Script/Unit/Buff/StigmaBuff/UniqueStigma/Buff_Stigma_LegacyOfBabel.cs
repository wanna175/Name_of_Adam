using UnityEngine;

public class Buff_Stigma_LegacyOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_LegacyOfBabel;

        _name = "LegacyOfBabel";

        _description = "자신의 턴마다 공격 타일에 오벨리스크를 설치합니다.";

        _owner = owner;

        _stigmataBuff = true;
    }
}
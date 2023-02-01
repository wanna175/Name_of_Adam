using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Dir
{
    Left  =  -1,
    Right =   1,
    Up    =  10,
    Down  = -10
}

[CreateAssetMenu(fileName = "Effect_Move", menuName = "Scriptable Object/Effect_Move", order = 2)]
public class Effect_Move : EffectSO
{
    [SerializeField] List<Dir> MoveDir;
    [SerializeField] Dictionary<string, Vector2> dic = new Dictionary<string, Vector2>();

    // 이동 실행
    public override void Effect(BattleUnit caster)
    {
        for (int i = 0; i < MoveDir.Count; i++)
        {
            int x = (int)MoveDir[i];
            int y = (int)MoveDir[i] / 10;

            //caster.MoveLotate(caster, x, y);
        }
    }
}

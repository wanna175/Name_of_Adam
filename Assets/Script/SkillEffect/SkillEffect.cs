using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().Play("SkillEffect");
    }

    public void AnimExit()
    {
        BattleManager.SkillEffect.RestoreSkillEffect(gameObject);
    }
}

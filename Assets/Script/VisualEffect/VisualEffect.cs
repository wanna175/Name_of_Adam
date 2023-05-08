using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().Play("SkillEffect");
    }

    public void AnimExit()
    {
        GameManager.VisualEffect.RestoreVisualEffect(gameObject);
    }
}

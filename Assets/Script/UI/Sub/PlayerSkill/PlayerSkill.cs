using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    public abstract string GetName();
    public abstract int GetManaCost();
    public abstract int GetDarkEssenceCost();
    public abstract void CancleSelect();
    public abstract void OnSelect();
}
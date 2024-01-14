using System;
using System.Collections;
using UnityEngine;

public class Stigma : MonoBehaviour
{
    [SerializeField] private StigmaEnum _stigmaEnum;
    public StigmaEnum StigmaEnum => _stigmaEnum;

    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField, TextArea] private string _description;
    public string Description => _description;

    [SerializeField] private ActiveTiming _activeTiming;
    public ActiveTiming ActiveTiming => _activeTiming;

    [SerializeField] private StigmaTier _tier;
    public StigmaTier Tier => _tier;

    [SerializeField] private bool _isLock = false;
    public bool IsLock => _isLock;
    

    public virtual void Use(BattleUnit caster = null)
    {
        StigmaEffect(caster);
        return;
    }

    public void UnlockStigma()
    {
        _isLock = false;
    }

    private void StigmaEffect(BattleUnit caster)
    {
        Vector3 pos = caster.transform.position + new Vector3(0, caster.transform.lossyScale.y * 0.5f, 0);
        GameManager.VisualEffect.StartStigmaEffect(Sprite, pos);
        return;
    }

}
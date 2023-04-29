using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TurnChangeButton : UI_Scene
{
    [SerializeField] private Button _changeButton;
    [SerializeField] private Text _text;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TurnChange()
    {
        PhaseController _phase = GameManager.Battle.Phase;

        if (_phase.Current == _phase.Prepare)
            _phase.ChangePhase(_phase.Engage);
        else if (_phase.Current == _phase.Move)
            _phase.ChangePhase(_phase.Action);
        else
            _phase.ChangePhase(_phase.Engage);
    }
}

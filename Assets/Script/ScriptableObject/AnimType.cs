using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationType", menuName = "Scriptable Object/AnimationType")]
public class AnimType : ScriptableObject
{
    [SerializeField] private CutSceneMoveType _moveType;
    public CutSceneMoveType MoveType => _moveType;

    [SerializeField] private float _zoomSize;
    public float ZoomSize => _zoomSize;

    [SerializeField] private float _gradientPower;
    public float GradientPower => _gradientPower;

}

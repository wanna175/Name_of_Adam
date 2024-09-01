using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/Unit")]
public class UnitDataSO : ScriptableObject
{
    // 변경되는 값들

    [SerializeField] private Stat _rawStat;
    public Stat RawStat => _rawStat;

    [SerializeField] private List<Stigma> _uniqueStigma = new ();
    public List<Stigma> UniqueStigma => _uniqueStigma;

    // 변경되지않는 값들

    [SerializeField] private string _id;
    public string ID => _id;

    [SerializeField] private string _name;
    public string Name => GameManager.Locale.GetLocalizedUnitName(_name);

    [SerializeField] private string _description;
    public string Description => _description;

    [SerializeField] private Rarity _rarity;
    public Rarity Rarity => _rarity;

    [SerializeField] private Sprite _image;
    public Sprite Image => _image;

    [SerializeField] private Sprite _corruptImage;
    public Sprite CorruptImage => _corruptImage;

    [SerializeField] private Sprite _portraitImage;
    public Sprite PortraitImage => _portraitImage;

    [SerializeField] private Sprite _corruptPortraitImage;
    public Sprite CorruptPortraitImage => _corruptPortraitImage;

    [SerializeField] private Sprite _diaPortraitImage;
    public Sprite DiaPortraitImage => _diaPortraitImage;

    [SerializeField] private Sprite _corruptDiaPortraitImage;
    public Sprite CorruptDiaPortraitImage => _corruptDiaPortraitImage;

    [SerializeField] private RuntimeAnimatorController _animatorController;
    public RuntimeAnimatorController AnimatorController => _animatorController;

    [SerializeField] private RuntimeAnimatorController _corruptionAnimatorController;
    public RuntimeAnimatorController CorruptionAnimatorController => _corruptionAnimatorController;

    [SerializeField] private AnimationClip _skillEffectAnim;
    public AnimationClip SkillEffectAnim => _skillEffectAnim;

    [SerializeField] private AnimationClip _corruptionSkillEffectAnim;
    public AnimationClip CorruptionSkillEffectAnim => _corruptionSkillEffectAnim;

    [SerializeField] private AnimType _animType;
    public AnimType AnimType => _animType;

    [SerializeField] private int _darkEssenseDrop;
    public int DarkEssenseDrop => _darkEssenseDrop;

    [SerializeField] private int _darkEssenseCost;
    public int DarkEssenseCost => _darkEssenseCost;

    [SerializeField] private UnitActionType _unitActionType;
    public UnitActionType UnitActionType => _unitActionType;

    [SerializeField] private UnitMoveType _unitMoveType;
    public UnitMoveType UnitMoveType => _unitMoveType;

    [SerializeField] bool _isBattleOnly;
    public bool IsBattleOnly => _isBattleOnly;

    [SerializeField] bool _isFlipFixed;
    public bool IsFlipFixed => _isFlipFixed;

    const int Arow = 5;
    const int Acolumn = 11;

    const int Mrow = 5;
    const int Mcolumn = 5;

    [SerializeField] [HideInInspector] private bool[] _attackRange = new bool[Arow * Acolumn];
    public bool[] AttackRange => _attackRange;
    [SerializeField] [HideInInspector] private bool[] _moveRange = new bool[Mrow * Mcolumn];
    public bool[] MoveRange => _moveRange;
    [SerializeField] [HideInInspector] private bool[] _splashRange = new bool[Arow * Acolumn];
    public bool[] SplashRange => _splashRange;
    [SerializeField] [HideInInspector] private bool[] _unitSize = new bool[Mrow * Mcolumn];
    public bool[] UnitSize => _unitSize;
}   
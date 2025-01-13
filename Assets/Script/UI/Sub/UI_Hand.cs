using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_Hand : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _inactive;
    [SerializeField] private Image _unitImage;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private GameObject _darkessense;
    [SerializeField] private TextMeshProUGUI _darkessensecost;

    private DeckUnit _handUnit = null;
    private UI_Hands _hands;

    private UI_Info _hoverInfo;
    private UI_Info _selectInfo;

    public bool IsSelected = false;
    
    private void Start()
    {
        _highlight.SetActive(false);
        BattleManager.Mana.ManaInableCheck();
    }

    public void SetUnit(UI_Hands hands, DeckUnit unit)
    {
        _hands = hands;
        _handUnit = unit;
        SetUnitInfo();
    }

    public void SetUnitInfo()
    {
        _unitImage.sprite = _handUnit.Data.CorruptPortraitImage;
        _cost.text = _handUnit.DeckUnitTotalStat.ManaCost.ToString();

        if (_handUnit.Data.DarkEssenseCost > 0)
        {
            _darkessense.SetActive(true);
            _darkessensecost.gameObject.SetActive(true);
            _darkessensecost.text = _handUnit.Data.DarkEssenseCost.ToString();
        }
            
        if (_handUnit.IsDiscount())
        {
            _cost.color = Color.yellow;
            _darkessensecost.color = Color.yellow;
        }
        else
        {
            _cost.color = Color.white;
            _darkessensecost.color = Color.white;
        }
    }

    public DeckUnit GetUnit() => _handUnit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //_highlight.SetActive(true);
        transform.localScale = new Vector3 (1.15f, 1.15f, 1.15f);
        _hoverInfo = BattleManager.BattleUI.ShowInfo();
        _hoverInfo.SetInfo(_handUnit, Team.Player);

        if (_handUnit.IsDiscount())
            BattleManager.BattleUI.SetFirstTurnNotify(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleManager.BattleUI.CloseInfo(_hoverInfo);
        if (IsSelected)
            return;
        transform.localScale = new Vector3(1f, 1f, 1f);
        //_highlight.SetActive(false);

        if (_handUnit.IsDiscount())
            BattleManager.BattleUI.SetFirstTurnNotify(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PhaseController phase = BattleManager.Phase;
        if (!phase.CurrentPhaseCheck(phase.Prepare))
        {
            GameManager.Sound.Play("UI/UISFX/UIFailSFX");
            BattleManager.BattleUI.UI_controlBar.CreateSystemInfo(GameManager.Locale.GetLocalizedSystem("CannotInUnitTurn"));
            return;
        }

        if (BattleManager.PlayerSkillController.IsSkillOn)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (TutorialManager.Instance.IsEnableUpdate())
                TutorialManager.Instance.ShowNextTutorial();

            _hands.OnClickHand(this);
        }
    }

    public void ChangeSelectState(bool b)
    {
        IsSelected = b;
        _highlight.SetActive(b);
        if (!IsSelected)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            BattleManager.BattleUI.SetFirstTurnNotify(false);
        }

        /*
        if (IsSelected)
        {
            _selectInfo = BattleManager.BattleUI.ShowInfo();
            _selectInfo.SetInfo(_handUnit, Team.Player);
        }
        else
        {
            BattleManager.BattleUI.CloseInfo(_selectInfo);
        }
        */
    }
    
    public void ChangeInable(bool b)
    {
        _inactive.SetActive(b);
    }
    
}

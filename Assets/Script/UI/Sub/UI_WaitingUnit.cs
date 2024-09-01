using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_WaitingUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BattleUnit _unit;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _background;

    private Vector2? _prevHighlightLocation;

    private Color32 _enemy = new(166, 148, 93, 255);
    private Color32 _player = new(65, 57, 69, 255);

    public void SetUnit(BattleUnit unit)
    {
        _unit = unit;
        if (unit.Team == Team.Player)
        {
            _unitImage.sprite = unit.Data.CorruptPortraitImage;
            _background.color = _player;
        }
        else
        {
            _unitImage.sprite = unit.Data.PortraitImage;
            _background.color = _enemy;

            _unitImage.transform.eulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    public BattleUnit GetUnit() => _unit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _prevHighlightLocation = BattleManager.Field.NowHighlightFrameOnLocation;
        if (!_unit.Location.Equals(new Vector2(-1, -1)))
        {
            BattleManager.Field.SetTileHighlightFrame(_unit.Location, true);
            BattleManager.Field.FieldShowInfo(BattleManager.Field.TileDict[_unit.Location]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_unit.Location.Equals(new Vector2(-1, -1)))
        {
            BattleManager.Field.SetTileHighlightFrame(_prevHighlightLocation, true);
            BattleManager.Field.FieldCloseInfo(BattleManager.Field.TileDict[_unit.Location]);
        }
    }
}

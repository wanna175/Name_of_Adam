using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_WaitingUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private (BattleUnit, int?) _unitOrder;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _background;

    private Vector2? _prevHighlightLocation;

    private Color32 _enemy = new(166, 148, 93, 255);
    private Color32 _player = new(65, 57, 69, 255);

    public void SetUnitOrder((BattleUnit, int?) unitOrder)
    {
        _unitOrder = unitOrder;
        if (_unitOrder.Item1.Team == Team.Player)
        {
            _unitImage.sprite = _unitOrder.Item1.Data.CorruptPortraitImage;
            _background.color = _player;
        }
        else
        {
            _unitImage.sprite = _unitOrder.Item1.Data.PortraitImage;
            _background.color = _enemy;

            _unitImage.transform.eulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    public (BattleUnit, int?) GetUnitOrder() => _unitOrder;

    public void SetAnimatorBool(string name, bool on)
    {
        GetComponent<Animator>().SetBool(name, on);
    }

    public void RemoveAnimationEnd()
    {
        Destroy(this.gameObject);
    }

    public void DisableAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _prevHighlightLocation = BattleManager.Field.NowHighlightFrameOnLocation;
        if (!_unitOrder.Item1.Location.Equals(new Vector2(-1, -1)))
        {
            BattleManager.Field.SetTileHighlightFrame(_unitOrder.Item1.Location, true);
            BattleManager.Field.FieldShowInfo(BattleManager.Field.TileDict[_unitOrder.Item1.Location]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_unitOrder.Item1.Location.Equals(new Vector2(-1, -1)))
        {
            BattleManager.Field.SetTileHighlightFrame(_prevHighlightLocation, true);
            BattleManager.Field.FieldCloseInfo(BattleManager.Field.TileDict[_unitOrder.Item1.Location]);
        }
    }
}

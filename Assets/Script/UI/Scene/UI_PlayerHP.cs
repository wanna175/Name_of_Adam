using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlayerHP : UI_Scene, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image profile;
    [SerializeField] private Image HPBackImage, HPEffectImage;
    [SerializeField] private GameObject[] HPJemImages;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        // 프로필 설정
        profile.sprite = GameManager.Data.GameData.Incarna.Sprite;

        // HP 설정
        for (int i = 0; i < HPJemImages.Length; i++)
        {
            if (i < GameManager.Data.GameData.PlayerHP)
                HPJemImages[i].SetActive(true);
            else
                HPJemImages[i].SetActive(false);
        }
    }

    public void DecreaseHP(int hp)
    {
        for (int i = 0; i < hp; i++) 
        {
            GameManager.Data.GameData.PlayerHP -= 1;
            PlayerHPEffect effect = HPJemImages[GameManager.Data.GameData.PlayerHP].GetComponent<PlayerHPEffect>();
            if (effect == null) 
            {
                Debug.LogError("PlayerHPEffect 참조 실패");
                return;
            }
            effect.StartDecreaseHPEffect();
        }
    }

    public void StartEffect() => BattleManager.BattleUI.UI_animator.SetBool("isPlayerHit", true);

    private bool _isHover = false;
    private bool _isHoverMessegeOn = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
        BattleManager.Instance.PlayAfterCoroutine(() => {
            if (_isHover)
            {
                _isHoverMessegeOn = true;
                GameManager.UI.ShowHover<UI_TextHover>().SetText(
                    $"{GameManager.Locale.GetLocalizedBattleScene("PlayerHP UI Info")}", Input.mousePosition);
            }
        }, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;

        if (_isHoverMessegeOn)
        {
            _isHoverMessegeOn = false;
            GameManager.UI.CloseHover();
        }
    }
}

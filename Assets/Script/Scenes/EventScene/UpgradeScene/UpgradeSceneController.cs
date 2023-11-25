using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum CUR_EVENT {
    UPGRADE=1,
    RELEASE,
    COMPLETE_UPGRADE,
    COMPLETE_RELEASE,//<===complete 하나로도 충분히 될것 같은데???
    STIGMA,
    GIVE_STIGMA,
    RECEIVE_STIGMA,
    COMPLETE_STIGMA
};
public class UpgradeSceneController : MonoBehaviour
{
    private DeckUnit _unit;

    //[SerializeField] private Image _upgradeunitImage; // 강화 대상 유닛

    //[SerializeField] private Image _releaseunitImage; // 교화 해소 유닛

    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    public static bool isEnd;
    public bool isReleaseSelect { get; set; }
    public bool isUpgradeSelect { get; set; }
    private List<Script> scripts;
    private UI_Conversation script = null;
    void Start()
    {
        isEnd = false;
        Init();
    }
    private void Update()
    {
        if (_unit != null && isUpgradeSelect)
            OnUpgradeButtonClick();
        if (_unit != null && isReleaseSelect)
            OnReleaseButtonClick();
        if (isEnd)
        {
            isEnd = false;
            //script.gameObject.SetActive(true);
            StartCoroutine(QuitScene());
        }
    }

    private void Init()
    {
        isUpgradeSelect = false;
        isReleaseSelect = false;
        isEnd = false;
        
        scripts = new List<Script>();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
            scripts = GameManager.Data.ScriptData["강화소_입장_최초"];
        else
            scripts = GameManager.Data.ScriptData["강화소_입장"];

        //GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
        /*
        if(GameManager.Data.GameData)
        {
            _forbiddenButton.gameObject.SetActive(true);
        }
        else
        {
            _forbiddenButton.gameObject.SetActive(false);
        }
        */
    }

    // 업그레이드 할 유닛을 고릅니다.
    public void OnUpgradeUnitButtonClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectUpgrade,(int)CUR_EVENT.UPGRADE);
        isUpgradeSelect = true;
    }

    // 교화를 풀 유닛을 고릅니다.
    public void OnReleaseUnitButtonClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectRelease,(int)CUR_EVENT.RELEASE);
        isReleaseSelect = true;
    }

    //대화하기 버튼
    public void OnConversationButtonClick()
    {
        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
    }
    // 유닛 선택 후 업그레이드 완료 버튼//나는 안쓴다 나중에 지워버리자.
    public void OnUpgradeButtonClick()
    {
        isUpgradeSelect = false;
        if (_unit != null)
            GameManager.UI.ShowPopup<UI_UpgradeSelectButton>().Init(this);
        //GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
    }

    // 유닛 선택 후 교화 풀기 버튼
    public void OnReleaseButtonClick()
    {
        isReleaseSelect = false;
        if (_unit.DeckUnitChangedStat.FallCurrentCount > 0)
        {
            _unit.DeckUnitChangedStat.FallCurrentCount -= 1;
        }
        UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        unitInfo.SetUnit(_unit);
        unitInfo.Init(null, (int)CUR_EVENT.COMPLETE_RELEASE);
        //StartCoroutine(QuitScene());

    }

    public void OnSelectUpgrade(DeckUnit unit)
    {
        //_upgradeunitImage.gameObject.SetActive(true);
        _unit = unit;
        //_upgradeunitImage.sprite = unit.Data.Image;
        //_upgradeunitImage.color = Color.white;

        
        //GameManager.UI.ClosePopup();
        //GameManager.UI.ClosePopup();
        //GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectUpgrade);
    }

    public void OnSelectRelease(DeckUnit unit)
    {
        //_releaseunitImage.gameObject.SetActive(true);
        _unit = unit;
        //_releaseunitImage.sprite = unit.Data.Image;
        //_releaseunitImage.color = Color.white;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
    }

    public void OnUpgradeSelect(int select)
    {
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        
        //script = GameManager.UI.ShowPopup<UI_Conversation>();
        //script.gameObject.SetActive(false);

        if (select == 1)
        {
            _unit.DeckUnitUpgradeStat.ATK += 5;
          // script.Init(GameManager.Data.ScriptData["강화소_공격력"], false);
        }
        else if (select == 2)
        {
            _unit.DeckUnitUpgradeStat.MaxHP += 15;
            //script.Init(GameManager.Data.ScriptData["강화소_체력"], false);
        }
        else if (select == 3)
        {
            _unit.DeckUnitUpgradeStat.SPD += 25;
           // script.Init(GameManager.Data.ScriptData["강화소_속도"], false);
        }
        else if (select == 4)
        {
            _unit.DeckUnitUpgradeStat.ManaCost -= 10;
            //script.Init(GameManager.Data.ScriptData["강화소_코스트"], false);
        }
        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
        /*GameObject go = GameManager.VisualEffect.StartVisualEffect(
            Resources.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UpgradeEffect"),
            _upgradeunitImage.transform.position);*/
        UI_UnitInfo _UnitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        _UnitInfo.SetUnit(_unit);
        _UnitInfo.Init(null, (int)CUR_EVENT.COMPLETE_UPGRADE);
        //StartCoroutine(QuitScene(script));
    }

    public void OnQuitClick()
    {
        StartCoroutine(QuitScene());
        if (_unit != null)
            GameManager.UI.ClosePopup();
        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            GameManager.Data.GameData.isVisitUpgrade = true;
        }
    }

    private IEnumerator QuitScene(UI_Conversation eventScript = null)
    {
        if (GameManager.Data.GameData.isVisitStigma == false)
        {
            GameManager.Data.GameData.isVisitStigma = true;
        }

        if (eventScript != null)
            yield return StartCoroutine(eventScript.PrintScript());

        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            GameManager.Data.GameData.isVisitUpgrade = true;
            quitScript.Init(GameManager.Data.ScriptData["강화소_퇴장_최초"], false);
        }
        else
            quitScript.Init(GameManager.Data.ScriptData["강화소_퇴장"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.SaveManager.SaveGame();
        SceneChanger.SceneChange("StageSelectScene");
    }
}
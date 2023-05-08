using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 전투를 담당하는 매니저
// 필드와 턴의 관리
// 필드에 올라와있는 캐릭터의 제어를 배틀매니저에서 담당

public class BattleManager : MonoBehaviour
{
    private static BattleManager s_instance;
    public static BattleManager Instance { get { Init(); return s_instance; } }

    private SoundManager _sound;
    public static SoundManager Sound => Instance._sound;

    [SerializeField] CutSceneController _cutScene;
    public static CutSceneController CutScene => Instance._cutScene;

    private BattleDataManager _battleData;
    public static BattleDataManager Data => Instance._battleData;

    private Field _field;
    public static Field Field => Instance._field;

    private Mana _mana;
    public static Mana Mana => Instance._mana;

    private PhaseController _phase;
    public static PhaseController Phase => Instance._phase;

    private UI_TurnChangeButton _turnChangeButton;

    private List<BattleUnit> hitUnits;
    private Vector2 coord;

    public FieldColorType fieldColorType = FieldColorType.none;

    private void Awake()
    {
        _turnChangeButton = GameManager.UI.ShowScene<UI_TurnChangeButton>();
        _battleData = Util.GetOrAddComponent<BattleDataManager>(gameObject);
        _mana = Util.GetOrAddComponent<Mana>(gameObject);
        _phase = new PhaseController();
        GameManager.Sound.Play("BattleBGMA", Sounds.BGM);
    }

    private void Update()
    {
        _phase.OnUpdate();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@BattleManager");

            if (go == null)
            {
                //go = new GameObject("@BattleManager");
                //go.AddComponent<BattleManager>();
                return;
            }

            s_instance = go.GetComponent<BattleManager>();
        }
    }

    public void SetupField()
    {
        GameObject fieldObject = GameObject.Find("Field");

        if (fieldObject == null)
            fieldObject = GameManager.Resource.Instantiate("Field");


        _field = fieldObject.GetComponent<Field>();
    }

    public void SpawnInitialUnit()
    {
        GetComponent<UnitSpawner>().SpawnInitialUnit();
    }

    public void ChangeButtonName()
    {
        TextMeshProUGUI buttonName = _turnChangeButton.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        if (Phase.Current == Phase.Prepare)
            buttonName.text = "Next Turn";
        else if (Phase.Current == Phase.Engage)
            buttonName.text = "";
        else if (Phase.Current == Phase.Move)
            buttonName.text = "Move Skip";
        else if (Phase.Current == Phase.Action)
            buttonName.text = "Action Skip";
    }

    public void MovePhase()
    {
        BattleUnit unit = Data.GetNowUnit();

        if (Field.Get_Abs_Pos(unit, FieldColor.Move).Contains(coord) == false)
            return;
        Vector2 dest = coord - unit.Location;

        
        MoveLocate(unit, dest); //이동시 낙인 체크

        _phase.ChangePhase(_phase.Action);
    }

    public void ActionPhase()
    {
        BattleUnit unit = Data.GetNowUnit();

        if (Field.Get_Abs_Pos(unit, FieldColor.Attack).Contains(coord) == false)
            return;

        if (coord != unit.Location)
        {
            List<Vector2> splashRange = unit.GetSplashRange(coord, unit.Location);
            List<BattleUnit> unitList = new List<BattleUnit>();

            foreach (Vector2 splash in splashRange)
            {
                BattleUnit targetUnit = Field.GetUnit(coord + splash);

                if (targetUnit == null)
                    continue;

                // 힐러의 예외처리 필요
                if(targetUnit.Team != unit.Team)
                    unitList.Add(targetUnit);
            }


            AttackStart(unit, unitList);
        }
    }

    public void EngagePhase()
    {
        Field.ClearAllColor();

        if (Data.OrderUnitCount <= 0)
        {
            _phase.ChangePhase(_phase.Prepare);
            return;
        }

        BattleUnit unit = Data.GetNowUnit();
        if (unit.Team == Team.Enemy)
        {
            unit.AI.AIAction();
            return;
        }

        _phase.ChangePhase(_phase.Move);
    }

    public void StartPhase()
    {
        if (Field._coloredTile.Count <= 0)
            return;

        if (fieldColorType == FieldColorType.UnitSpawn)
        {
            //SpawnUnitOnField(true);
            SpawnUnitOnField();
        }
    }

    public void PreparePhase()
    {
        if (fieldColorType == FieldColorType.UnitSpawn)
        {
            SpawnUnitOnField();
        }
        else if (fieldColorType == FieldColorType.FallPlayerSkill)
        {
            FallUnitOnField();
        }
    }

    private void SpawnUnitOnField(bool isFrist=false)
    {
        DeckUnit unit = Data.UI_hands.GetSelectedUnit();
        if (Field._coloredTile.Contains(coord) == false)
            return;
        BattleUnit spawnedUnit = GetComponent<UnitSpawner>().DeckSpawn(unit, coord);
        
        if (isFrist)
            Mana.ChangeMana(-1 * ((unit.Stat.ManaCost + 1) / 2));
        else
            Mana.ChangeMana(-unit.Stat.ManaCost);

        spawnedUnit.PassiveCheck(spawnedUnit, null, PassiveType.SUMMON); //배치 시 낙인 체크
        Data.RemoveHandUnit(unit);
        GameManager.UI.ClosePopup();
        Field.ClearAllColor();
    }

    private void FallUnitOnField()
    {
        if (Field._coloredTile.Contains(coord) == false)
            return;
        
        Mana.ChangeMana(-20);
        Data.DarkEssenseChage(-1);
        GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        Field.GetUnit(coord).ChangeFall(1);
        _battleData.UI_PlayerSkill.CancleSelect();
        _battleData.UI_PlayerSkill.Used = true;
        Field.ClearAllColor();
        BattleOverCheck();
    }

    public void OnClickTile(Tile tile)
    {
        coord = Field.FindCoordByTile(tile);
        _phase.OnClickEvent();
    }

    public void UnitSetting(BattleUnit _unit, Vector2 coord, Team team)
    {
        _unit.SetTeam(team);
        Field.EnterTile(_unit, coord);
        _unit.UnitDeadAction = UnitDeadAction;

        Data.BattleUnitAdd(_unit);
        //Data.BattleUnitOrder();
    }

    public IEnumerator UnitAttack()
    {
        UnitAttackAction();
        yield return StartCoroutine(CutScene.AfterAttack());
        
        EndUnitkAction();
    }

    public void AttackStart(BattleUnit caster, BattleUnit hit)
    {
        List<BattleUnit> hits = new List<BattleUnit>();
        hits.Add(hit);

        hitUnits = hits;
        CutScene.BattleCutScene(caster, hitUnits);
    }
    public void AttackStart(BattleUnit caster, List<BattleUnit> hits)
    {
        hitUnits = hits;
        CutScene.BattleCutScene(caster, hitUnits);
    }

    // 애니메이션용 추가
    private void UnitAttackAction()
    {
        BattleUnit unit = Data.GetNowUnit();
        
        foreach (BattleUnit hit in hitUnits)
        {
            if (hit == null)
                continue;
            Team team = hit.Team;
            Debug.Log(team);
            //공격 전 낙인 체크
            unit.SkillUse(hit);

            if (unit.SkillEffectAnimator != null)
                GameManager.VisualEffect.StartVisualEffect(unit.SkillEffectAnimator, hit.transform.position);

            if (team != hit.Team)
                hit.ChangeHP(1000);

            Debug.Log(hit.Team);
            if (hit.HP.GetCurrentHP() <= 0)
                continue;

            unit.PassiveCheck(unit, hit, PassiveType.AFTERATTACK);
        }

        string unitname = unit.DeckUnit.Data.Name;
        string faction = unit.DeckUnit.Data.Faction.ToString();
        Debug.Log(unitname + "   " + faction);
        GameManager.Sound.Play("Character/" + faction + "/" + unitname + "/" + unitname + "_Attack");

    }

    public void EndUnitkAction()
    {
        Field.ClearAllColor();
        Data.BattleOrderRemove(Data.GetNowUnit());
        BattleOverCheck();
        _phase.ChangePhase(_phase.Engage);
        Data.UI_DarkEssence.Refresh(); //이 코드를 발견했다면 수정해야하는 임시 코드이니 알릴 것
    }

    private void UnitDeadAction(BattleUnit _unit)
    {
        Data.BattleUnitRemove(_unit);
        Data.BattleOrderRemove(_unit);
    }

    public void DirectAttack()//임시 삭제
    {
        int randNum = UnityEngine.Random.Range(0, Data.PlayerHands.Count);
        Data.RemoveHandUnit(Data.PlayerHands[randNum]);
    }

    public void BattleOverCheck()
    {
        int MyUnit = 0;
        int EnemyUnit = 0;

        foreach (BattleUnit BUnit in Data.BattleUnitList)
        {
            if (BUnit.Team == Team.Player)//아군이면
                MyUnit++;
            else
                EnemyUnit++;
        }

        //MyUnit += Data.PlayerDeck.Count;
        //MyUnit += Data.PlayerHands.Count;
        //EnemyUnit 대기 중인 리스트만큼 추가하기

        if (MyUnit == 0)
        {
            BattleOverLose();
        }
        else if (EnemyUnit == 0)
        {
            BattleOverWin();
        }
    }

    private void BattleOverWin()
    {
        Debug.Log("YOU WIN");
        Data.OnBattleOver();
        _phase.ChangePhase(new BattleOverPhase());
        GameManager.UI.ShowScene<UI_BattleOver>().SetImage(1);
    }

    private void BattleOverLose()
    {
        Debug.Log("YOU LOSE");
        _phase.ChangePhase(new BattleOverPhase());
        GameManager.UI.ShowScene<UI_BattleOver>().SetImage(3);
        GameManager.Data.DeckClear();
    }

    // 이동 경로를 받아와 이동시킨다
    private void MoveLocate(BattleUnit caster, Vector2 coord)
    {
        Vector2 current = caster.Location;
        Vector2 dest = current + coord;

        Field.MoveUnit(current, dest);
        GameManager.Sound.Play("Move/MoveSFX");
    }

    public enum FieldColorType
    {
        none,
        UnitSpawn,
        FallPlayerSkill
    }

    public bool UnitSpawnReady(bool b)
    {
        if (_phase.Current != _phase.Prepare)
            return false;

        if (b)
        {
            Field.SetTileColor();
            fieldColorType = FieldColorType.UnitSpawn;
        }
        else
        {
            Field.ClearAllColor();
            fieldColorType = FieldColorType.none;
        }

        return true;
    }

    public bool FallPlayerSkillReady(bool b)
    {
        if (_phase.Current != _phase.Prepare)
            return false;

        if (b)
        {
            Field.SetEnemyUnitTileColor();
            fieldColorType = FieldColorType.FallPlayerSkill;
        }
        else
        {
            Field.ClearAllColor();
            fieldColorType = FieldColorType.none;
        }

        return true;
    }

    public List<BattleUnit> GetArroundUnits(List<Vector2> coords)
    {
        List<BattleUnit> units = new List<BattleUnit>();

        foreach (Vector2 coord in coords)
        {
            BattleUnit targetUnit = Field.GetUnit(coord);
            if (targetUnit == null)
                continue;
            units.Add(targetUnit);
        }    

        return units;
    }
}
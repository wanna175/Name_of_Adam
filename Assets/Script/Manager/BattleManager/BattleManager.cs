using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전투를 담당하는 매니저
// 필드와 턴의 관리
// 필드에 올라와있는 캐릭터의 제어를 배틀매니저에서 담당

public class BattleManager : MonoBehaviour
{
    private static BattleManager s_instance;
    public static BattleManager Instance { get { Init(); return s_instance; } }

    private SoundManager _sound;
    public static SoundManager Sound => Instance._sound;

    [SerializeField] BattleCutSceneController _battlecutScene;
    public static BattleCutSceneController BattleCutScene => Instance._battlecutScene;

    [SerializeField] UnitSpawner _spawner;
    public static UnitSpawner Spawner => Instance._spawner;

    private BattleDataManager _battleData;
    public static BattleDataManager Data => Instance._battleData;

    private BattleUIManager _battleUI;
    public static BattleUIManager BattleUI => Instance._battleUI;

    private PlayerSkillController _playerSkillController;
    public static PlayerSkillController PlayerSkillController => Instance._playerSkillController;

    private Field _field;
    public static Field Field => Instance._field;

    private Mana _mana;
    public static Mana Mana => Instance._mana;

    private PhaseController _phase;
    public static PhaseController Phase => Instance._phase;

    [SerializeField] List<GameObject> Background;

    private UnitIDManager _unitIDManager;
    public static UnitIDManager UnitIDManager => Instance._unitIDManager;

    private void Awake()
    {
        _battleData = Util.GetOrAddComponent<BattleDataManager>(gameObject);
        _battleUI = Util.GetOrAddComponent<BattleUIManager>(gameObject);
        _mana = Util.GetOrAddComponent<Mana>(gameObject);
        _phase = new PhaseController();
        _playerSkillController = Util.GetOrAddComponent<PlayerSkillController>(gameObject);
        _unitIDManager = new UnitIDManager();
        _unitIDManager.Init(GameManager.Data.GetDeck());
        SetBackground();
        Time.timeScale = GameManager.OutGameData.Data.BattleSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _phase.CurrentPhaseCheck(Phase.Prepare))
        {
            //우클릭
            if (GameManager.OutGameData.Data.TutorialClear)
                _battleUI.CancelAllSelect();
        }

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
        GameManager.Instance.PlayAfterCoroutine(() => {
            _spawner.SpawnInitialUnit();
            SpawnDivineCheck();
        }, 0.5f);

        GameManager.Instance.PlayAfterCoroutine(() => {
            StageData currentStageData = GameManager.Data.Map.GetCurrentStage();

            if ((currentStageData.Name == StageName.EliteBattle || currentStageData.Name == StageName.BossBattle) &&
            GameManager.Data.GameData.CurrentAct != 99)
            {
                string unitName = GameManager.Data.StageDatas[currentStageData.StageLevel][currentStageData.StageID].Units[0].Name;

                if (GameManager.OutGameData.DialogPlayCheck(unitName))
                {
                    List<Script> scripts = GameManager.Data.ScriptData[unitName];
                    GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts, true, true);
                    GameManager.OutGameData.SetDialogData(unitName, true);

                    return;
                }
            }

            _phase.ChangePhase(_phase.Prepare);
        }, 1f);
    }

    public void SpawnDivineCheck()
    {
        if (GameManager.Data.Map.GetCurrentStage().Name == StageName.BossBattle || 
            GameManager.Data.Map.GetCurrentStage().Name == StageName.EliteBattle)
            return;

        Dictionary<int, int> threshold = new() {
            {0, 0}, {11, 0}, {12, 0}, {13, 20}, {14, 40},
            {21, 0}, {22, 20}, {23, 30}, {24, 50},
            {31, 0}, {32, 3}, {33, 50}, {34, 70},
            {91, 0}, {92, 50}, {93, 70}, {94, 100},
        };

        BattleUnit buffUnit = Data.BattleUnitList[UnityEngine.Random.Range(0, Data.BattleUnitList.Count)];
        Vector3 stageDivine = GameManager.Data.GameData.StageDivine;
        
        // 튜토리얼 반영
        if (TutorialManager.Instance.IsTutorialOn())
        {
            buffUnit = Data.BattleUnitList[0];
            GameManager.Data.GameData.StageDivine = new(1,0,0);
        }

        if (stageDivine.x == 1 && stageDivine.y == 1 && stageDivine.z == 1)
        {
            GameManager.Data.GameData.StageDivine = new(-1,0,0);
        }
        else if (stageDivine.x == -1 && stageDivine.y == -1)
        {
            buffUnit.SetBuff(new Buff_Divine());

            GameManager.Data.GameData.StageDivine = new(1,0,0);
        }
        else if (UnityEngine.Random.Range(0, 100) < threshold[GameManager.Data.Map.GetCurrentStage().StageLevel % 100])
        {
            buffUnit.SetBuff(new Buff_Divine());
            GameManager.Data.GameData.StageDivine = new(1, stageDivine.x, stageDivine.y);
        }
        else
        {
            GameManager.Data.GameData.StageDivine = new(-1, stageDivine.x, stageDivine.y);
        }
    }

    private void SetBackground()
    {
        StageData currentStage = GameManager.Data.Map.GetCurrentStage();
        int stageID = currentStage.StageID;

        if (GameManager.Data.StageAct == 2)
        {
            StageData data = GameManager.Data.Map.GetStage(99);
            string unitName = GameManager.Data.StageDatas[data.StageLevel][data.StageID].Units[0].Name;

            Background[3].SetActive(unitName == "욘");
            Background[2].SetActive(unitName == "구원자");
            Background[1].SetActive(unitName == "바누엘");
            Background[0].SetActive(false);
        }
        else
        {
            Background[0].SetActive(true);
        }
    }

    #region Click 관련
    private bool _tileClickCooldown = false;
    private int _cooldownCounter = 0;

    public void OnClickTile(Tile tile)
    {
        if (!_tileClickCooldown && Data.IsCorruptionPopupOn == false)
        {
            Vector2 coord = _field.GetCoordByTile(tile);
            _phase.OnClickEvent(coord);
        }
    }

    public void SetTlieClickCoolDown(float time)
    {
        _tileClickCooldown = true;
        _cooldownCounter++;

        GameManager.Instance.PlayAfterCoroutine(() => {
            _cooldownCounter--;
            if (_cooldownCounter == 0)
            {
                _tileClickCooldown = false;
            }
        }, time);
    }

    public void PreparePhaseClick(Vector2 coord)
    {
        if (!_field.TileDict[coord].IsColored)
        {
            if (!PlayerSkillController.IsSkillOn)
                _battleUI.CancelAllSelect();
            return;
        }

        if (PlayerSkillController.IsSkillOn)
        {
            PlayerSkillController.ActionSkill(ActiveTiming.TURN_START, coord);
            return;
        }

        SetTlieClickCoolDown(0.2f);

        if (_field.FieldType == FieldColorType.UnitSpawn)
        {
            SpawnUnitOnField(coord);
        }
        else if (_field.FieldType == FieldColorType.PlayerSkill)
        {
            _playerSkillController.PlayerSkillUse(coord);
        }
    }

    private void SpawnUnitOnField(Vector2 coord)
    {
        if (!_field.TileDict[coord].IsColored)
            return;

        DeckUnit unit = _battleUI.UI_hands.GetSelectedUnit();

        if (TutorialManager.Instance.IsEnableUpdate())
            TutorialManager.Instance.ShowNextTutorial(); 

        _mana.ChangeMana(-unit.DeckUnitTotalStat.ManaCost); //마나 사용가능 체크
        _battleData.DarkEssenseChage(-unit.Data.DarkEssenseCost);

        unit.FirstTurnDiscountUndo();

        _spawner.DeckSpawn(unit, coord);

        _battleUI.RemoveHandUnit(unit); //유닛 리필
        GameManager.UI.ClosePopup();
        _field.ClearAllColor();
    }

    public void MovePhaseClick(Vector2 coord)
    {
        if (!_field.TileDict[coord].IsColored)
            return;

        BattleUnit unit = _battleData.GetNowUnit();

        if (MoveUnit(unit, coord))
        {
            unit.IsDoneMove = true;
            _phase.ChangePhase(_phase.Action);
            SetTlieClickCoolDown(0.4f);
        }
        else if (coord == unit.Location)
        {
            _phase.ChangePhase(_phase.Action);
            SetTlieClickCoolDown(0.2f);
        }
        else
            GameManager.Sound.Play("UI/UISFX/UIFailSFX");

    }

    public void ActionPhaseClick(Vector2 coord)
    {
        if (!_field.TileDict[coord].IsColored)
            return;  

        if (!GameManager.OutGameData.Data.TutorialClear)
            TutorialManager.Instance.DisableToolTip();

        SetTlieClickCoolDown(0.2f);

        BattleUnit nowUnit = _battleData.GetNowUnit();

        List<Vector2> attackCoords = new();
        List<BattleUnit> unitList = new();
        attackCoords.Add(coord);
        
        if (nowUnit.DeckUnit.CheckStigma(StigmaEnum.AdditionalPunishment))
        {
            BattleUnit selectUnit = _field.GetUnit(coord);
            if (selectUnit == null || selectUnit.Team == Team.Player)
            {
                GameManager.Sound.Play("UI/UISFX/UIFailSFX");
                return;
            }

            List<BattleUnit> additionalEnemies = _field.GetUnitsInRange(nowUnit.Location, nowUnit.GetAttackRange(), Team.Enemy);
            additionalEnemies.Remove(selectUnit);
            if (additionalEnemies.Count > 0)
            {
                BattleUnit additionalEnemy = additionalEnemies[UnityEngine.Random.Range(0, additionalEnemies.Count)];
                attackCoords.Add(additionalEnemy.Location);
            }
        }

        foreach (var attackCoord in attackCoords)
        {
            if (_field.GetUnit(attackCoord) != nowUnit)
            {
                List<Vector2> splashRange = nowUnit.GetSplashRange(attackCoord, nowUnit.Location);
                
                foreach (Vector2 splash in splashRange)
                {
                    BattleUnit targetUnit = _field.GetUnit(attackCoord + splash);

                    if (targetUnit == null)
                        continue;

                    // 힐러의 예외처리 필요
                    if (targetUnit.Team != nowUnit.Team)
                        unitList.Add(targetUnit);
                }
            }
        }

        if (!nowUnit.Action.ActionStart(nowUnit, unitList, coord))
        {
            GameManager.Sound.Play("UI/UISFX/UIFailSFX");
            return;
        }

        if (Phase.CurrentPhaseCheck(Phase.Action) && nowUnit != null && unitList.Count > 0)
            BattleUI.UI_TurnChangeButton.SetEnable(false);
    }

    #endregion

    public void AttackStart(BattleUnit caster, BattleUnit hit)
    {
        List<BattleUnit> hits = new ();
        hits.Add(hit);

        AttackStart(caster, hits);
    }

    public void AttackStart(BattleUnit caster, List<BattleUnit> hits, bool isFlipFixed = false)
    {
        BattleCutSceneData CSData = new(caster, hits);
        CSData.IsFlipFixed = isFlipFixed;
        _battlecutScene.InitBattleCutScene(CSData);
        caster.AttackUnitNum = hits.Count;
        caster.IsDoneAttack = true;

        StartCoroutine(_battlecutScene.AttackCutScene(CSData));
    }

    public void AttackPlayer(BattleUnit caster)
    {
        BattleCutSceneData CSData = new(caster, new List<BattleUnit> { Data.IncarnaUnit });
        _battlecutScene.InitBattleCutScene(CSData);

        StartCoroutine(_battlecutScene.AttackCutScene(CSData));

        ActiveTimingCheck(ActiveTiming.BEFORE_ATTACK, caster);
        ActiveTimingCheck(ActiveTiming.DAMAGE_CONFIRM, caster);
        ActiveTimingCheck(ActiveTiming.AFTER_ATTACK, caster);
    }

    public void EndUnitAction()
    {
        _field.ClearAllColor();
        _battleData.RemoveCurrentTurnOrder();
        _battleUI.UI_darkEssence.Refresh();
        _phase.ChangePhase(_phase.Engage);
        BattleOverCheck();
    }

    public void StigmaSelectEvent(Corruption cor)
    {
        BattleUnit targetUnit = cor.GetTargetUnit();

        if (targetUnit.Fall.IsEdified)
        {
            cor.LoopExit();
            targetUnit.DeckUnit.ClearStigma();
        }
        else if (targetUnit.Data.Rarity == Rarity.Boss || targetUnit.Data.IsBattleOnly)
        {
            cor.LoopExit();
        }
        else
        {
            GameObject.Find("@UI_Root").transform.Find("UI_StigmaSelectBlocker").gameObject.SetActive(true);
            UI_StigmaSelectButtonPopup popup = GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>();

            popup.Init(targetUnit.DeckUnit, false, GameManager.Data.StigmaController.GetRandomStigmaList(targetUnit.DeckUnit, 2), cor.LoopExit);

            popup.gameObject.SetActive(false);
            Data.CorruptionPopups.Add(popup);
        }
    }

    public bool IsExistedCorruptionPopup() => Data.CorruptionPopups.Count != 0;

    public UI_StigmaSelectButtonPopup ShowLastCorruptionPopup()
    {
        foreach (var item in Data.CorruptionPopups)
            item.gameObject.SetActive(false);
        var popup = Data.CorruptionPopups[Data.CorruptionPopups.Count - 1];
        popup.gameObject.SetActive(true);
        Data.IsCorruptionPopupOn = true;
        return popup;
    }

    public void DirectAttack(BattleUnit attackUnit)
    {
        if (attackUnit.Buff.CheckBuff(BuffEnum.Stun))
        {
            BattleManager.Instance.EndUnitAction();
            return;
        }

        AttackPlayer(attackUnit);
    }

    public void UnitSummonEvent(BattleUnit unit)
    {
        _battleData.BattleOrderInsert(0, unit);
        _battleData.BattleUnitOrderSorting();
        FieldActiveEventCheck(ActiveTiming.FIELD_UNIT_SUMMON, unit);
    }

    public void UnitDeadEvent(BattleUnit unit)
    {
        _battleData.BattleUnitList.Remove(unit);
        if (unit.Location != new Vector2(-1,-1))
            _field.ExitTile(unit.Location);
        _battleData.BattleUnitRemoveFromOrder(unit);

        if (unit.Team == Team.Enemy && !unit.IsConnectedUnit)
        {
            if (GameManager.OutGameData.Data.IsVisitBaptism)
                GameManager.OutGameData.Data.BaptismCorruptValue++;
            
            if(unit.Data.Rarity == Rarity.Normal)
            {
                GameManager.Data.GameData.Progress.NormalKill++;
            }
            else if(unit.Data.Rarity == Rarity.Elite)
            {
                GameManager.Data.GameData.Progress.EliteKill++;
                switch (unit.DeckUnit.Data.ID)
                {
                    case "투발카인": GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_TUBALCAIN); break;
                    case "라헬&레아": GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_RAHELLEA); break;
                    case "엘리우스": GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_ELIEUS); break;
                    case "야나": GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_YANA); break;
                    case "압바임": GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_APPAIM); break;
                    case "리비엘": GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_LIBIEL); break;
                    case "아라벨라": GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_ARABELLA); break;
                }
            }
            else if (unit.DeckUnit.Data.Rarity == Rarity.Boss)
            {
                switch (unit.DeckUnit.Data.ID)
                {
                    case "바누엘":
                        GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_PHANUEL);
                        if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.Phanuel_Dead))
                        {
                            GameManager.OutGameData.SetCutSceneData(CutSceneType.Phanuel_Dead, true);
                            BattleCutSceneManager.Instance.StartCutScene(CutSceneType.Phanuel_Dead);
                            GameManager.Sound.Play("CutScene/Phanuel_Dead", Sounds.BGM);
                        }
                        GameManager.Data.GameData.Progress.PhanuelKill++;
                        break;
                    case "구원자":
                        GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_THESAVIOR);
                        if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.TheSavior_Dead))
                        {
                            GameManager.OutGameData.SetCutSceneData(CutSceneType.TheSavior_Dead, true);
                            BattleCutSceneManager.Instance.StartCutScene(CutSceneType.TheSavior_Dead);
                        }
                        GameManager.Data.GameData.Progress.SaviorKill++;
                        break;
                    case "욘":
                        GameManager.Steam.IncreaseAchievement(SteamAchievementType.KILL_YOHRN);
                        if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.Yohrn_Dead))
                        {
                            GameManager.OutGameData.SetCutSceneData(CutSceneType.Yohrn_Dead, true);
                            BattleCutSceneManager.Instance.StartCutScene(CutSceneType.Yohrn_Dead);
                        }
                        GameManager.Data.GameData.Progress.YohrnKill++;
                        break;
                    default: Debug.Log($"{unit.DeckUnit.Data.ID} 보스 컷씬 출력 실패"); break;
                }
            }

            GameManager.Data.DarkEssenseChage(unit.Data.DarkEssenseDrop);
        }

        FieldActiveEventCheck(ActiveTiming.FIELD_UNIT_DEAD, unit);

        BossDeadCheck(unit);
    }

    public void UnitFallEvent(BattleUnit unit)
    {
        if (GameManager.OutGameData.Data.IsVisitSacrifice)
            GameManager.OutGameData.Data.SacrificeCorruptValue++;

        if (unit.Team == Team.Enemy && !unit.Data.IsBattleOnly)
        {
            GameManager.Data.GameData.FallenUnits.Add(unit.DeckUnit);

            if (unit.DeckUnit.Data.Rarity == Rarity.Normal)
            {
                GameManager.Data.GameData.Progress.NormalFall++;
            }
            else if (unit.DeckUnit.Data.Rarity == Rarity.Elite)
            {
                GameManager.Data.GameData.Progress.EliteFall++;
            }
            else if (unit.DeckUnit.Data.ID == "바누엘")
            {
                GameManager.Data.GameData.Progress.PhanuelFall++;
            }
            else if (unit.DeckUnit.Data.ID == "구원자")
            {
                GameManager.Data.GameData.Progress.SaviorFall++;
            }
            else if (unit.DeckUnit.Data.ID == "욘")
            {
                GameManager.Data.GameData.Progress.YohrnFall++;
            }
        }

        BossDeadCheck(unit);
        FieldActiveEventCheck(ActiveTiming.FIELD_UNIT_FALLED, unit);
    }

    private void BossDeadCheck(BattleUnit unit)
    {
        if (unit.Team == Team.Enemy && (unit.Data.Rarity == Rarity.Elite || unit.Data.Rarity == Rarity.Boss))
        {
            bool isBossClear = true;
            foreach (BattleUnit remainUnit in _battleData.BattleUnitList)
            {
                if (((unit.Data.Rarity == Rarity.Elite && remainUnit.Data.Rarity != Rarity.Normal) || (unit.Data.Rarity == Rarity.Boss && remainUnit.Data.Rarity == Rarity.Boss))
                    && remainUnit.Team == Team.Enemy && !remainUnit.Fall.IsEdified && remainUnit != unit)
                {
                    isBossClear = false;
                    break;
                }
            }

            if (isBossClear)
            {
                while (true)
                {
                    BattleUnit remainUnit = _battleData.BattleUnitList.Find(findUnit => findUnit.Team == Team.Enemy && findUnit != unit && !findUnit.FallEvent);
                    if (remainUnit == null)
                        break;

                    remainUnit.UnitDiedEvent(false);
                }

                BattleOverCheck();
            }
        }
    }

    public void FieldActiveEventCheck(ActiveTiming timing, BattleUnit parameterUnit = null)
    {
        List<BattleUnit> checkEndList = new();

        while (true)
        {
            BattleUnit unit = _battleData.BattleUnitList.Find(x => !checkEndList.Contains(x));

            if (unit == null)
                break;

            checkEndList.Add(unit);
            ActiveTimingCheck(timing, unit, parameterUnit);
        }
    }

    public void BattleOverCheck()
    {
        if (Data.IsGameDone || BattleCutSceneManager.Instance.IsCutScenePlaying)
            return; // 컷씬 도중엔 체크하지 않음

        int EnemyUnit = 0;

        foreach (BattleUnit unit in Data.BattleUnitList)
        {
            if (unit.Team == Team.Enemy)//아군이면
                EnemyUnit++;
        }

        if (GameManager.Data.GameData.PlayerHP <= 0)
        {
            BattleOverLose();
            _unitIDManager.resetID();
        }
        else if (EnemyUnit == 0)
        {
            BattleOverWin();
            _unitIDManager.resetID();
        }
    }

    private void BattleOverWin()
    {
        Data.IsGameDone = true;
        _phase.ChangePhase(new BattleOverPhase());
        _battleData.OnBattleOver();
        Time.timeScale = 1f;

        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            StageData data = GameManager.Data.Map.GetCurrentStage();

            if (data.Name == StageName.BossBattle)
            {
                CheckBossCycle(data);

                GameManager.Data.GameData.Progress.BossWin++;
                GameManager.UI.ShowScene<UI_BattleOver>().SetImage("elite win");
                GameManager.SaveManager.DeleteSaveData();
            }
            else if (data.Name == StageName.EliteBattle)
            {
                GameManager.Data.GameData.Progress.EliteWin++;
                GameManager.UI.ShowScene<UI_BattleOver>().SetImage("elite win");
                GameManager.SaveManager.SaveGame();
            }
            else
            {
                GameManager.Data.GameData.Progress.NormalWin++;
                GameManager.UI.ShowScene<UI_BattleOver>().SetImage("win");
                GameManager.SaveManager.SaveGame();
            }

            GameManager.OutGameData.SaveData();
        }, 1f);
    }

    private void BattleOverLose()
    {
        Debug.Log("YOU LOSE");
        Data.IsGameDone = true;
        _phase.ChangePhase(new BattleOverPhase());
        BattleManager.BattleUI.UI_controlBar.UI_PlayerHP.StartDestoryEffect();
        GameManager.SaveManager.DeleteSaveData();

        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            GameManager.UI.ShowSingleScene<UI_BattleOver>().SetImage("lose");
        }, 1.5f);
    }

    private void CheckBossCycle(StageData data)
    {
        if (data.StageLevel == 214 && !GameManager.OutGameData.Data.PhanuelClear)
        {
            GameManager.OutGameData.Data.PhanuelClear = true;
        }
        else if (data.StageLevel == 224 && !GameManager.OutGameData.Data.SaviorClear)
        {
            GameManager.OutGameData.Data.SaviorClear = true;
        }
        else if (data.StageLevel == 234 && !GameManager.OutGameData.Data.YohrnClear)
        {
            GameManager.OutGameData.Data.YohrnClear = true;
            GameManager.Data.GameData.Progress.AllChapterClear = 1;
            }

        GameManager.OutGameData.SaveData();
    }

    // 이동 경로를 받아와 이동시킨다
    public bool MoveUnit(BattleUnit moveUnit, Vector2 dest, float moveSpeed = 1, bool isFlipFix = false)
    {
        Vector2 current = moveUnit.Location;

        if (!_field.IsInRange(dest) || current == dest)
            return false;

        if (moveUnit.ConnectedUnits.Count > 0)
        {
            if (_field.GetUnit(dest) != null)
                return false;

            Vector2 originalUnitMovePosition = dest;
            float minDistance = (dest - current).magnitude;

            foreach (ConnectedUnit unit in moveUnit.ConnectedUnits)
            {
                Vector2 unitMovePosition = dest - (unit.Location - current);
                float distance = (unitMovePosition - current).magnitude;

                if (distance < minDistance && _field.IsInRange(unitMovePosition))
                {
                    minDistance = distance;
                    originalUnitMovePosition = unitMovePosition;
                }
            }

            if (!_field.IsInRange(originalUnitMovePosition) ||
                  (_field.GetUnit(originalUnitMovePosition) != null && _field.GetUnit(originalUnitMovePosition).DeckUnit != moveUnit.DeckUnit))
                return false;

            foreach (ConnectedUnit unit in moveUnit.ConnectedUnits)
            {
                Vector2 unitDest = unit.Location + originalUnitMovePosition - current;
                if ((!_field.IsInRange(unitDest)) || 
                    (_field.GetUnit(unitDest) != null && _field.GetUnit(unitDest).DeckUnit != moveUnit.DeckUnit))
                    return false;
            }

            _field.ExitTile(current);

            foreach (ConnectedUnit unit in moveUnit.ConnectedUnits)
            {
                _field.ExitTile(unit.Location);
                _field.EnterTile(unit, unit.Location + originalUnitMovePosition - current);
                unit.SetLocation(unit.Location + originalUnitMovePosition - current);
            }

            _field.EnterTile(moveUnit, originalUnitMovePosition);
            moveUnit.UnitMove(originalUnitMovePosition, moveSpeed, isFlipFix);
        }
        else
        {
            if (_field.TileDict[dest].UnitExist)
            {
                BattleUnit destUnit = _field.TileDict[dest].Unit;

                if (Switchable(moveUnit, destUnit))
                {
                    _field.ExitTile(current);
                    _field.ExitTile(dest);

                    moveUnit.UnitMove(dest, moveSpeed, isFlipFix);
                    _field.EnterTile(moveUnit, dest);

                    destUnit.UnitMove(current, moveSpeed, false);
                    _field.EnterTile(destUnit, current);
                    ActiveTimingCheck(ActiveTiming.AFTER_SWITCH, moveUnit, destUnit);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (_field.IsInRange(current))
                    _field.ExitTile(current);
                _field.EnterTile(moveUnit, dest);
                moveUnit.UnitMove(dest, moveSpeed, isFlipFix);
            }
        }

        if (Phase.CurrentPhaseCheck(Phase.Move))
            BattleUI.UI_TurnChangeButton.SetEnable(false);
        
        GameManager.Sound.Play("Move/MoveSFX");

        return true;
    }

    private bool Switchable(BattleUnit moveUnit, BattleUnit destUnit) =>
        moveUnit.Team == destUnit.Team &&
        moveUnit.GetMoveRange().Contains(destUnit.Location - moveUnit.Location) &&
        destUnit.GetMoveRange().Contains(moveUnit.Location - destUnit.Location) &&
        moveUnit.Data.UnitMoveType != UnitMoveType.UnitMove_None &&
        destUnit.Data.UnitMoveType != UnitMoveType.UnitMove_None &&
        moveUnit.ConnectedUnits.Count == 0 &&
        destUnit.ConnectedUnits.Count == 0;

    public bool UnitSpawnReady(FieldColorType colorType, DeckUnit deckUnit = null)
    {
        if (!_phase.CurrentPhaseCheck(_phase.Prepare))
            return false;

        if (colorType == FieldColorType.none)
        {
            _field.ClearAllColor();
        }
        else if (colorType == FieldColorType.UnitSpawn)
        {
            _field.SetSpawnTileColor(colorType, deckUnit);
        }

        return true;
    }

    public void DivineCheck()
    {
        BattleUnit lastUnit = null;

        foreach (BattleUnit unit in Data.BattleUnitList)
        {
            if (unit.Team == Team.Enemy)
            {
                if (lastUnit != null)
                {
                    lastUnit = null;
                    break;
                }

                lastUnit = unit;
            }
        }

        if (lastUnit != null)
        {
            if (lastUnit.Buff.CheckBuff(BuffEnum.Divine) || lastUnit.Data.Rarity != Rarity.Normal)
                return;
            
            lastUnit.SetBuff(new Buff_Divine());
            
            if (TutorialManager.Instance.IsTutorialOn() && TutorialManager.Instance.CheckStep(TutorialStep.Popup_Last))
            {
                TutorialManager.Instance.ShowTutorial();
            }
        }
    }

    public bool ActiveTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver = null)
    {
        if (caster.IsConnectedUnit || receiver == Data.IncarnaUnit)
            return false;

        bool skipNextAction = false;

        foreach (Stigma stigma in caster.StigmaList)
        {
            if ((activeTiming & stigma.ActiveTiming) == activeTiming)
            {
                stigma.Use(caster);
            }
        }

        foreach (Buff buff in caster.Buff.CheckActiveTiming(activeTiming))
        {
            skipNextAction = buff.Active(receiver);
        }

        caster.Buff.CheckCountDownTiming(activeTiming);

        caster.BattleUnitChangedStat = caster.Buff.GetBuffedStat();

        caster.RefreshHPBar();

        skipNextAction |= caster.Action.ActionTimingCheck(activeTiming, caster, receiver);

        return skipNextAction;
    }
}
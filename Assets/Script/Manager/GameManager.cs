using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance { get { if (s_instance == null) { Init(); } return s_instance; } }

    [SerializeField] private UIManager _ui;
    public static UIManager UI => Instance._ui;

    [SerializeField] DataManager _data;
    public static DataManager Data => Instance._data;

    [SerializeField] private SoundManager _sound;
    public static SoundManager Sound => Instance._sound;

    private ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource => Instance._resource;

    [SerializeField] private InputManager _input;
    public static InputManager InputManager => Instance._input;

    [SerializeField] private VisualEffectManager _visualEffect;
    public static VisualEffectManager VisualEffect => Instance._visualEffect;

    [SerializeField] private SaveController _saveController;
    public static SaveController SaveManager => Instance._saveController;

    [SerializeField] private OutGameDataContainer _outGameData;
    public static OutGameDataContainer OutGameData => Instance._outGameData;

    [SerializeField] private LocaleManager _locale;
    public static LocaleManager Locale => Instance._locale;

    [SerializeField] private SteamClientManager _steam;
    public static SteamClientManager Steam => Instance._steam;

    [SerializeField] private TMPro.TMP_Text _systemInfoText;

    private readonly bool _onGM = true;

    private readonly bool _onDebug = false;

    void Awake()
    {
        if (s_instance != null)
            Destroy(gameObject); // 이미 GameManager가 있으면 이 오브젝트를 제거
        else
        {
            Init();
            /*
            SaveManager.Init();
            OutGameData.Init();
            Data.Init();
            Sound.Init();
            VisualEffect.Init();
            Locale.Init();
            Steam.Init();
            */
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");

            if (go == null)
            {
                go = new GameObject("@GameManager");
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();

            SaveManager.Init();
            OutGameData.Init();
            Data.Init();
            Sound.Init();
            VisualEffect.Init();
            Locale.Init();
            Steam.Init();

            s_instance._systemInfoText.SetText(string.Empty);
        }
    }

    public void SetSystemInfoText(string info)
    {
        if (!_onDebug)
            return;

        _systemInfoText.SetText($"[Debug Message] {info}");
    }

    private void Update()
    {
        if (!_onGM) // GM 모드가 꺼져있다면 리턴
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Time.timeScale = 4;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Time.timeScale = 0.2f;

        if (Input.GetKeyDown(KeyCode.O))
        {
            while (true)
            {
                BattleUnit unit = BattleManager.Data.BattleUnitList.Find(x => x.Team == Team.Enemy);
                if (unit == null)
                    break;

                unit.UnitDiedEvent();
            }
            BattleManager.Instance.BattleOverCheck();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (Stage stage in StageManager.Instance.CurrentStage.NextStage)
            {
                if (stage == null)
                {
                    GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID + 1);
                    GameManager.Data.Map.CurrentTileID = GameManager.Data.Map.CurrentTileID + 1;
                    return;
                }
                else
                {
                    GameManager.Data.Map.ClearTileID.Add(stage.Datas.ID);
                    GameManager.Data.Map.CurrentTileID = stage.Datas.ID;
                }
            }
            SceneChanger.SceneChange("StageSelectScene");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.OutGameData.Data.BaptismCorruptValue += 75;
            GameManager.OutGameData.Data.StigmataCorruptValue += 8;
            GameManager.OutGameData.Data.SacrificeCorruptValue += 13;

            Debug.Log($"타락도 조정: {GameManager.OutGameData.Data.BaptismCorruptValue}|" +
                $"{GameManager.OutGameData.Data.StigmataCorruptValue}|" +
                $"{GameManager.OutGameData.Data.SacrificeCorruptValue}");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UI_StigmaSelectButtonPopup stigmaPopup = GameObject.FindObjectOfType<UI_StigmaSelectButtonPopup>();
            if (stigmaPopup != null)
                stigmaPopup.ResetStigmataSelectButtons();

            UI_UpgradeSelectButtonPopup upgradePopup = GameObject.FindObjectOfType<UI_UpgradeSelectButtonPopup>();
            if (upgradePopup != null)
                upgradePopup.ResetUpgradeSelectButtons();

            UI_EliteReward uI_EliteReward = GameObject.FindObjectOfType<UI_EliteReward>();
            if (uI_EliteReward != null)
                uI_EliteReward.SetRewardPanel();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            DeckUnit newUnit = new();
            newUnit.Data = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/믿음을_저버린_자");
            newUnit.IsMainDeck = false;
            newUnit.HallUnitID = -1;

            GameManager.Data.AddDeckUnit(newUnit);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            DeckUnit newUnit = new();
            newUnit.Data = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/욘");
            newUnit.IsMainDeck = false;
            newUnit.HallUnitID = -1;

            GameManager.Data.AddDeckUnit(newUnit);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnData spawnData = new();
            spawnData.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/욘");
            spawnData.location = new(5, 2);
            spawnData.team = Team.Enemy;

            BattleManager.Spawner.SpawnDataSpawn(spawnData);

            spawnData.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/집정관");
            spawnData.location = new(2, 2);
            spawnData.team = Team.Enemy;

            BattleManager.Spawner.SpawnDataSpawn(spawnData);

            spawnData.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/흑기사");
            spawnData.location = new(1, 2);
            spawnData.team = Team.Player;

            BattleManager.Spawner.SpawnDataSpawn(spawnData);

            while (true)
            {
                BattleUnit unit = BattleManager.Data.BattleUnitList.Find(x => x.Team == Team.Enemy && x.Data.ID != "욘" && x.Data.ID != "주시자" && x.Data.ID != "집정관");
                if (unit == null)
                    break;

                unit.UnitDiedEvent();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SpawnData spawnData = new();
            spawnData.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/리비엘");
            spawnData.location = new(4, 1);
            spawnData.team = Team.Enemy;

            BattleManager.Spawner.SpawnDataSpawn(spawnData);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject.Find("@GameManager").transform.Find("IngameDebugConsole").gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.OutGameData.Data.ProgressCoin += 10000;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            GameManager.Data.Map = new MapData();
            string mapFolderPath = "Prefabs/Stage/Maps/";
            mapFolderPath += "StageAct" + GameManager.Data.StageAct;

            GameObject[] maps = Resources.LoadAll<GameObject>(mapFolderPath);
            GameManager.Data.Map.MapObject = maps[mapnum];
            mapnum++;
            SceneChanger.SceneChange("StageSelectScene");
        }
    }

    int mapnum = 0;

    public static string CreatePrivateKey()
        => Guid.NewGuid().ToString();
}
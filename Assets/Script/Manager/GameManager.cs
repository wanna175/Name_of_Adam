using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    private static GameManager Instance { get { Init(); return s_instance; } }

    
    [SerializeField] BattleManager _battleMNG;
    public static BattleManager BattleMNG => Instance._battleMNG;
    
    [SerializeField] private CutSceneManager _CutSceneMNG;
    public static CutSceneManager CutSceneMNG => Instance._CutSceneMNG;

    StageManager _stageMNG;
    public static StageManager StageMNG => Instance._stageMNG;

    DataManager _dataMNG;
    public static DataManager DataMNG => Instance._dataMNG;

    SceneChanger _sceneChanger;
    public static SceneChanger SceneChanger => Instance._sceneChanger;

    [SerializeField] UIManager _UIMNG;
    public static UIManager UIMNG => Instance._UIMNG;

    void Awake()
    {
        Init();

        _dataMNG = new DataManager();
    }

    private void Update()
    {
        // 디버그용 입력기
        if (Input.GetMouseButtonDown(1))
        {
            BattleMNG.EngageStart();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //  SceneChanger.SceneChange("StageSelectScene");
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("GameManager");

            if (go == null)
            {
                go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();
        }
    }
}

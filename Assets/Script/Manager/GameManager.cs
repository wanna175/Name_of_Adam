using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance { get { Init(); return s_instance; } }
    
    [SerializeField] BattleManager _battleMNG;
    public static BattleManager BattleMNG => Instance._battleMNG;
    
    [SerializeField] private CutSceneManager _CutSceneMNG;
    public static CutSceneManager CutSceneMNG => Instance._CutSceneMNG;

    [SerializeField] UIManager _UIMNG;
    public static UIManager UIMNG => Instance._UIMNG;

    private ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource => Instance._resource;

    #region 현재 안 씀
    StageManager _stageMNG;
    public static StageManager StageMNG => Instance._stageMNG;

    DataManager _dataMNG = new DataManager();
    public static DataManager DataMNG => Instance._dataMNG;

    SceneChanger _sceneChanger;
    public static SceneChanger SceneChanger => Instance._sceneChanger;
    #endregion

    void Awake()
    {
        Init();
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
        }
    }

    private void Update()
    {
        // 디버그용 입력기
        if (Input.GetMouseButtonDown(1))
        {
            BattleMNG.PrepareEnd();
            BattleMNG.EngageStart();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //  SceneChanger.SceneChange("StageSelectScene");
        }
    }
}

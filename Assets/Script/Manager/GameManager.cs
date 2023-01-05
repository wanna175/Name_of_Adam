using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion
    #region BattleManager
    BattleManager _battleMNG;
    public BattleManager BattleMNG => _battleMNG;
    #endregion
    #region StageManager
    StageManager _StageMNG;
    public StageManager StageMNG => _StageMNG;
    #endregion
    #region DataManger
    DataManager _DataMNG;
    public DataManager DataMNG => _DataMNG;
    #endregion
    #region SceneChanger
    SceneChanger _SceneChanger;
    public SceneChanger SceneChanger => _SceneChanger;
    #endregion


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        _battleMNG = new BattleManager();
        _StageMNG = new StageManager();
        _DataMNG = new DataManager();
        _SceneChanger = new SceneChanger();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BattleMNG.TurnStart();
        }
        else if (Input.GetMouseButtonDown(2))
        {

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneChanger.SceneChange("StageSelectScene");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneChanger.SceneChange("SampleScene");
        }
    }
}

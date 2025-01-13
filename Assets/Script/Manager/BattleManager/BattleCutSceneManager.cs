using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BattleCutSceneManager : MonoBehaviour
{
    private static BattleCutSceneManager _instance;
    public static BattleCutSceneManager Instance
    {
        set
        {
            if (_instance == null)
                _instance = value;
        }
        get
        {
            return _instance;
        }
    }

    [SerializeField] private GameObject cutSceneGO;
    [SerializeField] private VideoPlayer video;

    private VideoClip videoClip;
    private CutSceneType cutSceneToDisplay;
    public bool IsCutScenePlaying;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cutSceneGO.SetActive(false);
    }

    public void StartCutScene(CutSceneType cutSceneType)
    {
        string language = "EN";
        if (GameManager.OutGameData.Data.Language == 1)
            language = "KR";

        cutSceneToDisplay = cutSceneType;
        videoClip = GameManager.Resource.Load<VideoClip>($"Video/VideoClip/{cutSceneToDisplay}_{language}");

        cutSceneGO.SetActive(true);
        IsCutScenePlaying = true;

        GameManager.Sound.Clear();

        if (cutSceneType == CutSceneType.Phanuel_Dead ||
            cutSceneType == CutSceneType.TheSavior_Dead ||
            cutSceneType == CutSceneType.Yohrn_Dead)
            GameManager.Sound.Play("CutScene/Boss_Dead", Sounds.BGM);
        else
            GameManager.Sound.Play($"CutScene/{cutSceneToDisplay}", Sounds.BGM);

        video.clip = videoClip;
        video.loopPointReached += EndReached;
        video.Play();

        Debug.Log($"{cutSceneToDisplay}_{language} �ƾ� ����");
    }

    public void SkipButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIImportantButtonSFX");
        EndReached(video);
    }

    private void EndReached(VideoPlayer vp)
    {
#if UNITY_EDITOR
        Debug.Log($"End BattleCutScene: {cutSceneToDisplay}");
#endif
        cutSceneGO.SetActive(false);
        IsCutScenePlaying = false;

        GameManager.OutGameData.SetCutSceneData(cutSceneToDisplay, true);
        BattleManager.Instance.BattleOverCheck();
    }
}

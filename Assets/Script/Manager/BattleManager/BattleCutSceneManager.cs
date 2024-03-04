using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static Unity.VisualScripting.Icons;

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
        if (GameManager.OutGameData.GetLanguage() == 1)
            language = "KR";

        cutSceneToDisplay = cutSceneType;
        videoClip = GameManager.Resource.Load<VideoClip>($"Video/VideoClip/{cutSceneToDisplay}_{language}");

        cutSceneGO.SetActive(true);
        IsCutScenePlaying = true;

        video.clip = videoClip;
        video.loopPointReached += EndReached;
        video.Play();

        Debug.Log($"{cutSceneToDisplay}_{language} ÄÆ¾À ½ÃÀÛ");
    }

    public void SkipButton() => EndReached(video);

    private void EndReached(VideoPlayer vp)
    {
#if UNITY_EDITOR
        Debug.Log($"End BattleCutScene: {cutSceneToDisplay}");
#endif
        cutSceneGO.SetActive(false);
        IsCutScenePlaying = false;

        BattleManager.Instance.BattleOverCheck();
    }
}

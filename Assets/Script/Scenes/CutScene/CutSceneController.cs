using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;

    private VideoClip videoClip;
    private CutSceneType cutSceneToDisplay;

    private void Start()
    {
        GameManager.Sound.Clear();
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");

        string language = "EN";
        if (GameManager.OutGameData.GetLanguage() == 1)
            language = "KR";

        cutSceneToDisplay = GameManager.Data.CutSceneToDisplay;
        videoClip = GameManager.Resource.Load<VideoClip>($"Video/VideoClip/{cutSceneToDisplay}_{language}");

        GameManager.Sound.Play($"CutScene/{cutSceneToDisplay}", Sounds.BGM);
        video.clip = videoClip;
        video.loopPointReached += EndReached;
        video.Play();

        Debug.Log($"{cutSceneToDisplay}_{language} ÄÆ¾À ½ÃÀÛ");
    }

    public void SceneChange() => SceneChanger.SceneChange("StageSelectScene");

    public void SkipButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        GameManager.Sound.Clear();
        SceneChange();
    }

    private void EndReached(VideoPlayer vp)
    {
#if UNITY_EDITOR
        Debug.Log($"End CutScene: {cutSceneToDisplay}");
#endif
        GameManager.Sound.Clear();
        SceneChange();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;

    private VideoClip videoClip;
    private RenderTexture renderTexture;
    private CutSceneType cutSceneToDisplay;

    private void Start()
    {
        GameManager.Sound.Clear();
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        //GameManager.Sound.Play("Stage_Transition/CutScene/CutSceneBGM");

        cutSceneToDisplay = GameManager.Data.CutSceneToDisplay;
        videoClip = GameManager.Resource.Load<VideoClip>($"Video/VideoClip/{cutSceneToDisplay}_VideoClip");
     
        video.clip = videoClip;
        video.loopPointReached += EndReached;
        video.Play();
    }

    public void SceneChange()
    {
        switch (cutSceneToDisplay)
        {
            case CutSceneType.Start: SceneChanger.SceneChange("StageSelectScene"); break;
            case CutSceneType.Tutorial: SceneChanger.SceneChange("StageSelectScene"); break;
        }
    }

    public void SkipButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        SceneChange();
    }

    private void EndReached(VideoPlayer vp)
    {
#if UNITY_EDITOR
        Debug.Log($"End CutScene: {cutSceneToDisplay}");
#endif
        SceneChange();
    }
}

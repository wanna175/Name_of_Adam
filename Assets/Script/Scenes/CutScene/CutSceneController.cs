using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;

    private void Start()
    {
        GameManager.Sound.Clear();
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        //GameManager.Sound.Play("Stage_Transition/CutScene/CutSceneBGM");

        video.loopPointReached += EndReached;
    }

    public void SceneChange()
    {
        SceneChanger.SceneChange("StageSelectScene");
    }

    public void SkipButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        SceneChanger.SceneChange("StageSelectScene");
    }

    private void EndReached(VideoPlayer vp)
    {
#if UNITY_EDITOR
        Debug.Log("End Cutscene!");
#endif
        SceneChange();
    }
}

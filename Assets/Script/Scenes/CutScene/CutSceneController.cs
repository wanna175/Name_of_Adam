using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneController : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;

    private VideoClip _videoClip;
    private CutSceneType _cutSceneToDisplay;

    private void Start()
    {
        GameManager.Sound.Clear();

        string language = "EN";
        if (GameManager.OutGameData.Data.Language == 1)
            language = "KR";

        _cutSceneToDisplay = GameManager.Data.CutSceneToDisplay;
        _videoClip = GameManager.Resource.Load<VideoClip>($"Video/VideoClip/{_cutSceneToDisplay}_{language}");
        GameManager.Sound.Clear();

        if (_cutSceneToDisplay == CutSceneType.Tubalcain_Enter ||
            _cutSceneToDisplay == CutSceneType.Elieus_Enter || 
            _cutSceneToDisplay == CutSceneType.Libiel_Enter ||
            _cutSceneToDisplay == CutSceneType.RahelLea_Enter ||
            _cutSceneToDisplay == CutSceneType.Appaim_Enter ||
            _cutSceneToDisplay == CutSceneType.Arabella_Enter)
            GameManager.Sound.Play("CutScene/Elite_Enter", Sounds.BGM);
        else if (_cutSceneToDisplay == CutSceneType.Phanuel_Enter ||
            _cutSceneToDisplay == CutSceneType.TheSavior_Enter ||
            _cutSceneToDisplay == CutSceneType.Yohrn_Enter)
            GameManager.Sound.Play("CutScene/Boss_Enter", Sounds.BGM);
        else if (_cutSceneToDisplay == CutSceneType.Phanuel_Dead ||
            _cutSceneToDisplay == CutSceneType.TheSavior_Dead ||
            _cutSceneToDisplay == CutSceneType.Yohrn_Dead)
            GameManager.Sound.Play("CutScene/Boss_Dead", Sounds.BGM);
        else if (_cutSceneToDisplay == CutSceneType.NPC_Baptism_Corrupt ||
            _cutSceneToDisplay == CutSceneType.NPC_Sacrifice_Corrupt ||
            _cutSceneToDisplay == CutSceneType.NPC_Stigmata_Corrupt)
            GameManager.Sound.Play("CutScene/NPC_Corrupt", Sounds.BGM);
        else if (_cutSceneToDisplay == CutSceneType.Main)
            GameManager.Sound.Play($"CutScene/Main", Sounds.BGM);

        video.clip = _videoClip;
        video.loopPointReached += EndReached;
        video.Play();

        Debug.Log($"{_cutSceneToDisplay}_{language} ƒ∆æ¿ Ω√¿€");
    }

    public void SceneChange()
    {
        // ESC∏¶ ƒ“ ªÛ≈¬∂Û∏È ESC∏¶ ≤Ù∞Ì æ¿¿ª ∫Ø∞Ê«’¥œ¥Ÿ.
        if (GameManager.UI.IsOnESCOption)
            GameManager.UI.CloseAllOption();

        if (_cutSceneToDisplay == CutSceneType.NPC_Baptism_Corrupt 
            || _cutSceneToDisplay == CutSceneType.NPC_Stigmata_Corrupt
            || _cutSceneToDisplay == CutSceneType.NPC_Sacrifice_Corrupt)
            SceneChanger.SceneChange("EventScene");
        else
            SceneChanger.SceneChange("StageSelectScene");
    }

    public void SkipButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIImportantButtonSFX");
        EndReached(video);
    }

    private void EndReached(VideoPlayer vp)
    {
#if UNITY_EDITOR
        Debug.Log($"End CutScene: {_cutSceneToDisplay}");
#endif
        GameManager.OutGameData.SetCutSceneData(_cutSceneToDisplay, true);
        GameManager.Sound.Clear();
        SceneChange();
    }
}

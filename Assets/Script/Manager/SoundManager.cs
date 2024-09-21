using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource[] _audioSources = new AudioSource[(int)Sounds.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        AudioMixer mixer = Resources.Load<AudioMixer>("Sounds/AudioMixer");

        string[] soundNames = System.Enum.GetNames(typeof(Sounds));
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();

            _audioSources[i].outputAudioMixerGroup = mixer.FindMatchingGroups(soundNames[i])[0];
            go.transform.parent = transform;
        }

        _audioSources[(int)Sounds.BGM].loop = true;
    }

    public void Clear()
    {
        for (int i = 0; i < _audioSources.Length; i++)
        {
            AudioSource audioSource = _audioSources[i];

            if (audioSource != null)
            {
                if (audioSource == _audioSources[(int)Sounds.BGM])
                {
                    audioSource.Stop();
                }
                audioSource.clip = null;
            }
        }
        _audioClips.Clear();
    }

    public void Play(string path, Sounds type = Sounds.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(type.ToString() + "/" + path, type);
        Debug.Log(type.ToString() + "/" + path);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Sounds type = Sounds.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Sounds.BGM)
        {
            AudioSource audioSource = _audioSources[(int)Sounds.BGM];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.volume = GetBGMVolume();
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Sounds.Effect];

            audioSource.volume = GetSEVolume();
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void SetSoundVolume(Sounds type)
    {
        if (type == Sounds.BGM)
        {
            AudioSource audioSource = _audioSources[(int)Sounds.BGM];
            audioSource.volume = GetBGMVolume();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Sounds.Effect];
            audioSource.volume = GetSEVolume();
        }
    }

    private float GetBGMVolume() => GameManager.OutGameData.Data.MasterSoundPower * GameManager.OutGameData.Data.BGMSoundPower;

    private float GetSEVolume() => GameManager.OutGameData.Data.MasterSoundPower * GameManager.OutGameData.Data.SESoundPower;

    AudioClip GetOrAddAudioClip(string path, Sounds type = Sounds.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Sounds.BGM)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = GameManager.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void SceneBGMPlay(string scenename)
    {
        if (scenename == "BattleScene")
        {
            Clear();
            Play("Stage_Transition/Stage_Enter/Stage_EnterSFX");
            if (GameManager.Data.Map.GetCurrentStage().Name == StageName.BossBattle)
            {
                StageData data = GameManager.Data.Map.GetStage(99);
                string unitName = GameManager.Data.StageDatas[data.StageLevel][data.StageID].Units[0].Name;

                if (unitName == "¹Ù´©¿¤")
                {
                    Play(scenename + "/BossBattle/Phanuel_BGM", Sounds.BGM);
                }
                else if (unitName == "±¸¿øÀÚ")
                {
                    Play(scenename + "/BossBattle/TheSavior_BGM", Sounds.BGM);
                }
                else if (unitName == "¿æ")
                {
                    Play(scenename + "/BossBattle/Yohrn_BGM", Sounds.BGM);
                }
            }
            else
            {
                Play(scenename + "/" + scenename + "BGM", Sounds.BGM);
            }
        }
        else if (scenename == "DifficultySelectScene")
        {
            Clear();
            Play("DifficultySelectScene/" + scenename + "BGM", Sounds.BGM);
        }
        else if (scenename == "EventScene")
        {
            Clear();
            string storeName = GameManager.Data.Map.GetCurrentStage().Name.ToString();
            Play("EventScene/" + storeName + "BGM", Sounds.BGM);
        }
        else if (scenename == "StageSelectScene")
        {
            Clear();
            Play(scenename + "/" + scenename + "BGM", Sounds.BGM);
        }
        else if (scenename != "LogoScene")
        {
            Clear();
            Play(scenename + "/" + scenename + "BGM", Sounds.BGM);
        }
    }
}

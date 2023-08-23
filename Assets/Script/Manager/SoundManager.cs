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
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string path, Sounds type = Sounds.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(type.ToString() + "/" + path, type);
        //Debug.Log(type.ToString() + "/" + path);
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

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Sounds.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

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
            Play(scenename + "/" + scenename + "BGM", Sounds.BGM);
        }
        else if(scenename == "EventScene")
        {
            Clear();
            string storeName = GameManager.Data.Map.GetCurrentStage().Name.ToString();
            Play(scenename + "/" + storeName);
        }
        else if(scenename == "CutScene")
        {
            Clear();
        }
        else if (scenename != "LogoScene")
        {
            Clear();
            Play(scenename + "/" + scenename + "BGM", Sounds.BGM);
        }
    }
   



}

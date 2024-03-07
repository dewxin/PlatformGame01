using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundManager:Singleton<SoundManager> 
{

    private const string EffectPath = "Sound/Effect";
    private const string MusicPath = "Sound/Music";

    private Dictionary<string, AudioClip> effectName2AudioClipDict = new Dictionary<string, AudioClip>();

    private AudioSource effectAudioSource = null;
    public AudioSource MusicAudioSource { get;private set; }

    public void SetBackgroundMusicSource(AudioSource audioSource)
    {
        MusicAudioSource = audioSource;
    }


    public void PlayEffect(string effectName)
    {
        if(!effectName2AudioClipDict.TryGetValue(effectName, out var audioClip))
        {
            audioClip = Resources.Load<AudioClip>(Path.Combine(EffectPath, effectName));
            effectName2AudioClipDict.Add(effectName, audioClip);
        }

        if(effectAudioSource == null)
            effectAudioSource = Camera.main.gameObject.AddComponent<AudioSource>();

        effectAudioSource.PlayOneShot(audioClip);
    }

    public void PlayMusic(string musicName)
    {
        var audioClip = Resources.Load<AudioClip>(Path.Combine(MusicPath, musicName));
        PlayMusic(audioClip);
    }

    //TODO 可能需要淡入淡出的效果
    public void PlayMusic(AudioClip musicClip)
    {
        if (MusicAudioSource == null)
            return;
        if(MusicAudioSource.clip.name== musicClip.name)
            return;

        MusicAudioSource.clip = musicClip;
        MusicAudioSource.Play();
    }

}

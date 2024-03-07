using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicSetting : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    void Start()
    {
        audioSource= GetComponent<AudioSource>();

        SoundManager.Instance.SetBackgroundMusicSource(audioSource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationFocus(bool focus)
    {
        if (audioSource==null)
            return;

        audioSource.mute = !focus;
    }
}

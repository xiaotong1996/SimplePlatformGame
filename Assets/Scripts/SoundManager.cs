using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        _instance = this;
    }

    public bool IsAudioPlay()
    {
        return audioSource.isPlaying;

    }

    public void PlayAudio(AudioClip ac)
    {
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
    }

    public void PlayAudioByName(string name)
    {
        audioSource.volume = 0.4f;
        
        AudioClip ac = Resources.Load<AudioClip>("Sounds/" + name);
        PlayAudio(ac);
    }

    public void PlayMusic(AudioClip ac)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        this.audioSource.clip = ac;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayMusicByName(string name)
    {
        audioSource.volume = 0.5f;
        AudioClip ac = Resources.Load<AudioClip>("Sounds/" + name);
        PlayMusic(ac);
    }

    public void StopPlay(string name)
    {
        audioSource.Stop();
    }


}

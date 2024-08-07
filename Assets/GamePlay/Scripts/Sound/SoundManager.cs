using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESound
{
    CollectBrick,
    BuildBridge,
    PlayerFall,
    PlayerWin,
    PlayerLose,
    GameMusic,
    GameCountDownStart,
    GameCountDownLoop,
    GameCountDownEnd
}


public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource;
    public SoundSO[] soundData;
    public AudioSourceLoop[] soundLoops;
    public AudioSource[] soundSources;
    private Queue<AudioSource> _queueSources;
    
    protected override void Awake()
    {
        base.Awake();
        OnInit();
    }

    public SoundSO GetSoundDataOfType(ESound soundType, bool isRandom)
    {
        var soundListRandom = new List<SoundSO>();
        foreach (var sound in soundData)
        {
            if (sound.soundType == soundType)
            {
                if (isRandom)
                {
                    soundListRandom.Add(sound);
                }
                else
                {
                    return sound;
                }
            }
        }

        if (isRandom)
        {
            return soundListRandom[Random.Range(0, soundListRandom.Count)];
        }

        return null;
    }

    public void OnInit()
    {
        _queueSources = new Queue<AudioSource>(soundSources);
        ChangeSoundVolume();
        ChangeMusicVolume();
    }

    public void ChangeSoundVolume()
    {
    }

    public void ChangeMusicVolume()
    {
    }

    public void PlayShotOneTime(SoundSO clip, float volume = 1f)
    {
        if (clip == null)
        {
            return;
        }

        bool hasPlay = false;

        foreach (var soundSrc in soundSources)
        {
            if (soundSrc.clip == clip.soundClip && soundSrc.isPlaying)
            {
                hasPlay = true;
                break;
            }
        }

        if (!hasPlay)
        {
            PlayShot(clip,volume);
        }
    }
    public void PlayShot(SoundSO clip, float volume = 1f)
    {
        if (clip == null)
        {
            return;
        }

        var source = _queueSources.Dequeue();
        if (!source)
        {
            return;
        }

        //source.volume = volume;
        source.clip = clip.soundClip;
        source.Play();
        //source.PlayOneShot(clip.soundClip);
        _queueSources.Enqueue(source);
    }

    public void StopLoop(ESound type)
    {
        foreach (var sound in soundLoops)
        {
            if (sound.CheckIsType(type))
            {
                sound.ActionSource(false);
            }
        }
    }

    public void PlayLoop(ESound type)
    {
        foreach (var sound in soundLoops)
        {
            if (sound.CheckIsType(type))
            {
                sound.ActionSource(true);
            }
        }
    }
}
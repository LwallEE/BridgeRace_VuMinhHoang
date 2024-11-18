using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESound
{
    CollectBrick=0,
    BuildBridge = 1,
    PlayerFall = 2,
    PlayerWin = 3,
    PlayerLose = 4,
    GameMusic = 5,
    GameCountDownStart = 6,
    GameCountDownLoop = 7,
    GameCountDownEnd = 8,
    ButtonClick= 9
}


public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource;
    public SoundSO[] soundData;
    public AudioSourceLoop[] soundLoops;
    public AudioSource[] soundSources;
    private Queue<AudioSource> _queueSources;
    private Dictionary<ESound, List<SoundSO>> numberOfEachEsoundDict;
    
    protected override void Awake()
    {
        base.Awake();
        OnInit();
    }

    public SoundSO GetSoundDataOfType(ESound soundType, bool isRandom)
    {
        if (!numberOfEachEsoundDict.ContainsKey(soundType)) return null;
        var list = numberOfEachEsoundDict[soundType];
        int endIndex = isRandom ? list.Count : 1;
        return list[Random.Range(0, endIndex)];
    }

    public void OnInit()
    {
        _queueSources = new Queue<AudioSource>(soundSources);
        InitNumberOfEachSoundDict();
        ChangeSoundVolume();
        ChangeMusicVolume();
    }

    void InitNumberOfEachSoundDict()
    {
        numberOfEachEsoundDict = new Dictionary<ESound, List<SoundSO>>();
        foreach (var item in soundData)
        {
            if (numberOfEachEsoundDict.ContainsKey(item.soundType))
            {
                numberOfEachEsoundDict[item.soundType].Add(item);
            }
            else
            {
                var newList = new List<SoundSO>(){item};
                numberOfEachEsoundDict.Add(item.soundType, newList);
            }
        }
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
    public void PlayShotOneTime(ESound type, float volume = 1f)
    {
        SoundSO so = GetSoundDataOfType(type, false);
        PlayShotOneTime(so, volume);
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
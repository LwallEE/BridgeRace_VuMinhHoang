using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.Barracuda.TextureAsTensorData;

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
    ButtonClick= 9,
    OnHit = 10,
    ButtonStep = 11,
    Correct = 12,
    Explose = 13,
    Impact = 14,
    Rocket = 15,
}


public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource;
    public SoundSO[] soundData;
    public AudioSourceLoop[] soundLoops;
    public AudioSource[] soundSources;
    private Queue<AudioSource> _queueSources;
    private Dictionary<ESound, List<SoundSO>> numberOfEachEsoundDict;

    public float SoundVolume {  get; private set; }
    public float MusicVolume { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        OnInit();
        
        SoundVolume = PlayerPrefs.GetFloat(Constants.KEY_VOLUME_SOUND, 1f);
        MusicVolume = PlayerPrefs.GetFloat(Constants.KEY_VOLUME_MUSIC, 1f);

        DontDestroyOnLoad(gameObject);
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
    public void ChangeSoundVolume(float volume)
    {
        SoundVolume = volume;
    }

    public void ChangeMusicVolume(float volume)
    {
        MusicVolume = volume;
        musicSource.volume = volume;
        for(int i = 0; i < soundLoops.Length; i++)
        {
            soundLoops[i].SetVolume(volume);
        }
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(Constants.KEY_VOLUME_SOUND, SoundVolume);
        PlayerPrefs.SetFloat(Constants.KEY_VOLUME_MUSIC, MusicVolume);
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
        SoundSO clip = GetSoundDataOfType(type, false);
        if (clip == null)
        {
            return;
        }
        PlayShot(clip, volume);
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

        source.volume = volume * SoundVolume;
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
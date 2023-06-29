using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    public AudioClip menu_musicClip;
    public AudioClip stage_0Clip;
    public AudioClip stage_1Clip;
    public AudioClip stage_2Clip;
    public AudioClip stage_3Clip;
    public AudioClip stage_4Clip;
    public AudioClip stage_5Clip;
    public AudioClip discoveredClip;
    public AudioClip alarm_1_5_barsClip;
    public AudioClip alarm_2_barsClip;

    public float playVolume;

    public AudioSource menu_music;
    public AudioSource stage_0;
    public AudioSource stage_1;
    public AudioSource stage_2;
    public AudioSource stage_3;
    public AudioSource stage_4;
    public AudioSource stage_5;
    public AudioSource discovered;
    public AudioSource alarm_1_5_bars;
    public AudioSource alarm_2_bars;
    [HideInInspector]
    public List<AudioSource> allSources = new List<AudioSource>();

    public Track currentTrack;

    private void Start()
    {
        UpdateSounds();
        StartTrack(Track.menu_music);
    }

    private void OnEnable()
    {
        DeleventSystem.levelInit += OnLevelInit;
        DeleventSystem.levelStart += OnLevelStart;
    }

    private void OnDisable()
    {
        DeleventSystem.levelInit -= OnLevelInit;
        DeleventSystem.levelStart -= OnLevelStart;
        
    }

    IEnumerator LevelMusicRoutine()
    {
        while (GameManager._instance != null)
        {
            //state machine for music being played
            Track trackToFadeTo = Track.stage_1;
            bool alarmedUnitFound = false;

            if (GameManager._instance.alarmSetOff)
            {
                if ((int)trackToFadeTo < (int)Track.alarm_2_bars)
                {
                    trackToFadeTo = Track.alarm_2_bars;
                }
                alarmedUnitFound = true;
            }

            foreach (var pos in UnitManager._instance.units)
            {
                foreach (var unit in pos.Value)
                {
                    if (unit is EnemyUnit)
                    {
                        EnemyUnit temp = (EnemyUnit)unit;
                        if(temp.state == EnemyState.Alarmed)
                        {
                            alarmedUnitFound = true;
                            if ((int)trackToFadeTo < (int)Track.discovered)
                            {
                                trackToFadeTo = Track.discovered;
                            }
                            break;
                        }
                    }
                }
                if (alarmedUnitFound)
                    break;
            }

            foreach (var unit in GameManager._instance.currentPlayerUnits)
            {
                if (MapManager._instance.GetSecurityLevel( unit.position) >= 3)
                {
                    if((int)trackToFadeTo < (int)Track.stage_4)
                    {
                        trackToFadeTo = Track.stage_4;
                    }
                }
                else if(MapManager._instance.GetSecurityLevel(unit.position) >= 2)
                {
                    if ((int)trackToFadeTo < (int)Track.stage_3)
                    {
                        trackToFadeTo = Track.stage_3;
                    }
                }
                else if(MapManager._instance.GetSecurityLevel(unit.position) >= 1)
                {
                    if ((int)trackToFadeTo < (int)Track.stage_2)
                    {
                        trackToFadeTo = Track.stage_2;
                    }
                }
            }

            if(trackToFadeTo != currentTrack)
            {
                FadeToTrack(trackToFadeTo);
            }
            yield return new WaitForSeconds(0.3f);
        }
        StartTrack(Track.menu_music);
    }

    void OnLevelStart()
    {
        StartTrack(Track.stage_0);
    }
    void OnLevelInit()
    {
        FadeToTrack(Track.stage_1);
        StartCoroutine(LevelMusicRoutine());        
    }

    void Testfade()
    {
        FadeToTrack(Track.stage_3);
    }
    void Testfade2()
    {
        FadeToTrack(Track.stage_5);
    }

    public void UpdateSounds()
    {
        foreach (var source in gameObject.GetComponents<AudioSource>())
        {
            DestroyImmediate(source);
        }
        if (menu_music == null)
            menu_music = gameObject.AddComponent<AudioSource>();
        if (stage_0 == null)
            stage_0 = gameObject.AddComponent<AudioSource>();
        if (stage_1 == null)
            stage_1 = gameObject.AddComponent<AudioSource>();
        if (stage_2 == null)
            stage_2 = gameObject.AddComponent<AudioSource>();
        if (stage_3 == null)
            stage_3 = gameObject.AddComponent<AudioSource>();
        if (stage_4 == null)
            stage_4 = gameObject.AddComponent<AudioSource>();
        if (stage_5 == null)
            stage_5 = gameObject.AddComponent<AudioSource>();
        if (discovered == null)
            discovered = gameObject.AddComponent<AudioSource>();
        if (alarm_1_5_bars == null)
            alarm_1_5_bars = gameObject.AddComponent<AudioSource>();
        if (alarm_2_bars == null)
            alarm_2_bars = gameObject.AddComponent<AudioSource>();

        allSources = new List<AudioSource>();
        allSources.Add(menu_music);
        allSources.Add(stage_0);
        allSources.Add(stage_1);
        allSources.Add(stage_2);
        allSources.Add(stage_3);
        allSources.Add(stage_4);
        allSources.Add(stage_5);
        allSources.Add(discovered);
        allSources.Add(alarm_1_5_bars);
        allSources.Add(alarm_2_bars);

        menu_music.clip = menu_musicClip;
        stage_0.clip = stage_0Clip;
        stage_1.clip = stage_1Clip;
        stage_2.clip = stage_2Clip;
        stage_3.clip = stage_3Clip;
        stage_4.clip = stage_4Clip;
        stage_5.clip = stage_5Clip;
        discovered.clip = discoveredClip;
        alarm_1_5_bars.clip = alarm_1_5_barsClip;
        alarm_2_bars.clip = alarm_2_barsClip;

        foreach (var source in allSources)
        {
            source.loop = true;
        }
    }

    public void MusicInit()
    {
        foreach (var source in allSources)
        {
            source.Play();
            source.volume = 0f;
        }
    }

    public void StartTrack(Track _track)
    {
        foreach (var source in allSources)
        {
            source.Play();
            source.volume = 0f;
        }
        GetTrackSource(_track).volume = playVolume;
        currentTrack = _track;
    }

    public void FadeToTrack(Track _track)
    {
        foreach (var source in allSources)
        {
            source.volume = 0f;
        }
        GetTrackSource(_track).volume = playVolume;
        currentTrack = _track;
    }

    public AudioSource GetTrackSource(Track _track)
    {
        switch (_track)
        {
            case Track.menu_music:
                return menu_music;
            case Track.stage_0:
                return stage_0;
            case Track.stage_1:
                return stage_1;
            case Track.stage_2:
                return stage_2;
            case Track.stage_3:
                return stage_3;
            case Track.stage_4:
                return stage_4;
            case Track.stage_5:
                return stage_5;
            case Track.discovered:
                return discovered;
            case Track.alarm_1_5_bars:
                return alarm_1_5_bars;
            case Track.alarm_2_bars:
                return alarm_2_bars;
            default:
                return
                    stage_0;
        }
    }
}

public enum Track
{
    menu_music,
    stage_0,
    stage_1,
    stage_2,
    stage_3,
    stage_4,
    stage_5,
    discovered,
    alarm_1_5_bars,
    alarm_2_bars
}

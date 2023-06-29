using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }


    public Sound[] sounds;
    public List<AudioSource> soundEffectSources;

    public List<LoopSound> loopSounds = new List<LoopSound>();
    private float Timer = 0;
    public void UpdateSounds()
    {
        foreach (var audioSource in gameObject.GetComponents<AudioSource>())
        {
            DestroyImmediate(audioSource);
        }

        //clear all audiosourcecomponents from go
        foreach (var sound in sounds)
        {
            if(sound.source != null)
                DestroyImmediate(sound.source);
        }

        //get all soundeffects from resource hierachy and create new Sound objects if not already there
        //all Sound objects should update their soundfile if they already existed


        //create new audioSources for each sound
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioFile;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
        }
    }

    public static Sound PlaySound(string name)
    {
        if(_instance == null)
        {
            //no soundmanager instance?
            return new Sound();
        }
        Sound s = Array.Find(_instance.sounds, sound => sound.soundName == name);
        if (s.source == null)
        {
            return s;
        }
        s.source.Play();
        return s;
    }
    public static Sound PlayLoop(string name, float time)
    {
        if (_instance == null)
        {
            //no soundmanager instance?
            return new Sound();
        }
        Sound s = Array.Find(_instance.sounds, sound => sound.soundName == name);
        if(s.source == null)
        {
            return s;
        }
        LoopSound loopSound = new LoopSound { source = s.source, timeExecute = _instance.Timer + time };
        _instance.loopSounds.Add(loopSound);
        s.source.loop = true;
        s.source.Play();
        return s;
    }
    void StopLoopSound(LoopSound loopSound)
    {
        loopSound.source.loop = false;
        loopSound.source.Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += 1 * Time.deltaTime;
        {
            List<LoopSound> deleteEvents = new List<LoopSound>();
            foreach (var item in loopSounds)
            {
                if (item.timeExecute <= Timer)
                {
                    StopLoopSound(item);
                    deleteEvents.Add(item);
                }
            }
            foreach (var item in deleteEvents)
            {
                loopSounds.Remove(item);
            }
        }
    }
}

public struct LoopSound
{
    public AudioSource source;
    public float timeExecute;
}

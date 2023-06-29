using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDataBase : ScriptableObject
{
    public AudioClip[] Sounds;
    public List<AudioSource> SoundEffectSources;
}

[Serializable]
public class Sound
{
    public string soundName;
    public AudioClip audioFile;
    //[HideInInspector]
    [Range(0.0f, 1.0f)]
    public float volume = 0.7f;
    //[HideInInspector]
    [Range(1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;
}

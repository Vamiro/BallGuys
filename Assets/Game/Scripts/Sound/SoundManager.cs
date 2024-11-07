using System;
using System.Collections.Generic;
using Game.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioCollection audioCollection;
    
    private readonly Dictionary<string, AudioCollection.Audio> _sounds = new();
    private readonly Dictionary<string, AudioCollection.Audio> _music = new();
    
    public AudioMixer AudioMixer => audioMixer;
    
    private void Awake()
    {
        base.Awake();
        foreach (var sound in audioCollection.soundList)
        {
            _sounds[sound.name] = sound;
        }
        
        foreach (var music in audioCollection.musicList)
        {
            _music[music.name] = music;
        }
    }
    
    public void PlaySound(string name)
    {
        if (!_sounds.TryGetValue(name, out var sound)) return;
        soundSource.PlayOneShot(sound.clip, sound.volume);
    }

    private void PlayMusic(string name)
    {
        if (!_music.TryGetValue(name, out var music)) return;
        if (musicSource.isPlaying) StopMusic();
        musicSource.clip = music.clip;
        musicSource.volume = music.volume;
        musicSource.Play();
    }

    private void StopMusic()
    {
        musicSource.Stop();
    }
}
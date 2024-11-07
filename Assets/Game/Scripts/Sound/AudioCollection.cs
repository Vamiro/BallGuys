using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundCollection", menuName = "Sound Collection", order = 1)]
public class AudioCollection : ScriptableObject
{
    [Serializable]
    public struct Audio
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
    }

    public List<Audio> soundList;
    public List<Audio> musicList;
}
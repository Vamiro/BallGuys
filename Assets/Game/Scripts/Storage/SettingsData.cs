using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsData : StorageData<SettingsData>
{
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float SFXlVolume = 1f;
    public float sensitivity = 1f;

    public void UpdateSettingsData()
    {
        SoundManager.Instance.AudioMixer.SetFloat("Master", LogarithmicValue(masterVolume));
        SoundManager.Instance.AudioMixer.SetFloat("Music", LogarithmicValue(musicVolume));
        SoundManager.Instance.AudioMixer.SetFloat("SFX", LogarithmicValue(SFXlVolume));
    }
    
    private static float CorrectValue(float rawValue) => Mathf.Max(rawValue, 0.0001f);
    private static float LogarithmicValue(float rawValue) => Mathf.Log(CorrectValue(rawValue)) * 20f;
}
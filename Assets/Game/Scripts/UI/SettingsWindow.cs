using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsWindow : BaseWindow<SettingsData>
{
    [SerializeField] private SliderVisualizerComponent masterVolumeSlider;
    [SerializeField] private SliderVisualizerComponent musicVolumeSlider;
    [SerializeField] private SliderVisualizerComponent soundVolumeSlider;
    [SerializeField] private SliderVisualizerComponent sensitivitySlider;
    [SerializeField] private Button backButton;
    [SerializeField] private LaunchWindow launchWindow;
    public UnityAction onBack;
    public UnityAction<SettingsData> onChange;

    private void Awake()
    {
        SettingsData.Instance.UpdateSettingsData();
        masterVolumeSlider.slider.onValueChanged.AddListener(OnChangeSlider);
        musicVolumeSlider.slider.onValueChanged.AddListener(OnChangeSlider);
        soundVolumeSlider.slider.onValueChanged.AddListener(OnChangeSlider);
        sensitivitySlider.slider.onValueChanged.AddListener(OnChangeSlider);
        backButton.onClick.AddListener(BackButtonClicked);
    }

    private void BackButtonClicked()
    {
        SoundManager.Instance.PlaySound("ButtonClick");
        Hide();
        launchWindow.Show();
    }

    protected override void SetUp(SettingsData argument)
    {
        masterVolumeSlider.SetSliderValue(argument.masterVolume);
        musicVolumeSlider.SetSliderValue(argument.musicVolume);
        soundVolumeSlider.SetSliderValue(argument.SFXlVolume);
        sensitivitySlider.SetSliderValue(argument.sensitivity);
    }
    
    private void OnChangeSlider(float _)
    {
        _argument.masterVolume = masterVolumeSlider.slider.value;
        _argument.musicVolume = musicVolumeSlider.slider.value;
        _argument.SFXlVolume = soundVolumeSlider.slider.value;
        _argument.sensitivity = sensitivitySlider.slider.value;
        SettingsData.Instance.UpdateSettingsData();
        onChange?.Invoke(_argument);
    }

    protected override void OnShow()
    {
    }

    protected override void OnHide()
    {
        SettingsData.Instance.Save();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(() => onBack?.Invoke());
    }
}

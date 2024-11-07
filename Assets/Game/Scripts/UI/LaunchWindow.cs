using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class LaunchWindow : BaseWindow
{
    private const string PlayerNamePrefKey = "PlayerName";
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button trainButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private SettingsWindow settingsWindow;
    
    public Button PlayButton => playButton;
    public UnityAction OnPlay;
    
    public Button TrainButton => trainButton;
    public UnityAction OnTrain;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayButtonClicked);
        trainButton.onClick.AddListener(TrainButtonClicked);
        exitButton.onClick.AddListener(ExitButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
        playerNameInputField.onDeselect.AddListener(OnPlayerNameFieldDeselected);
    }

    private void SettingsButtonClicked()
    {
        HandleButtonClick(() =>
        {
            Hide();
            settingsWindow.Show(SettingsData.Instance);
        });
    }

    private void PlayButtonClicked()
    {
        HandleButtonClick(OnPlay);
    }

    private void TrainButtonClicked()
    {
        HandleButtonClick(OnTrain);
    }

    private void ExitButtonClicked()
    {
        HandleButtonClick(Application.Quit);
    }

    private void HandleButtonClick(UnityAction action)
    {
        SoundManager.Instance.PlaySound("ButtonClick");
        try
        {
            action?.Invoke();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Start()
    {
        InitializeNameField();
    }

    protected override void OnShow()
    {
        
    }
    
    private void OnPlayerNameFieldDeselected(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            value = "Player" + Random.Range(1000, 10000);
        }
        
        playerNameInputField.text = value;
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(PlayerNamePrefKey,value);
    }

    private void InitializeNameField()
    {
        var nickName = PlayerPrefs.GetString(PlayerNamePrefKey);
        if (nickName.IsNullOrEmpty())
        {
            nickName = "Player" + Random.Range(1000, 10000);
            PlayerPrefs.SetString(PlayerNamePrefKey, nickName);
        }
        
        playerNameInputField.text = nickName;
        PhotonNetwork.NickName = nickName;
    }
    

    protected override void OnHide()
    {
        
    }
    
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(PlayButtonClicked);
        trainButton.onClick.RemoveListener(TrainButtonClicked);
        exitButton.onClick.RemoveListener(ExitButtonClicked);
        playerNameInputField.onDeselect.RemoveListener(OnPlayerNameFieldDeselected);
    }
}

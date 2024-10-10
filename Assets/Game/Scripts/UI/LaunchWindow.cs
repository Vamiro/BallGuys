using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LaunchWindow : BaseWindow
{
    [SerializeField] private Button playButton;
    [SerializeField] private Launcher launcher;
    [SerializeField] private TMP_InputField playerNameInputField;

    const string playerNamePrefKey = "PlayerName";

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        playerNameInputField.onValueChanged.AddListener(OnPlayerNameFieldChanged);
    }

    private void Start()
    {
        InitializeNameField();
    }

    protected override void OnShow()
    {
        
    }
    
    private void OnPlayButtonClicked()
    {
        launcher.Connect();
    }
    
    public void OnPlayerNameFieldChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey,value);
    }

    public void InitializeNameField()
    {
        string defaultName = string.Empty;
        if (playerNameInputField!=null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                playerNameInputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName =  defaultName;
    }

    protected override void OnHide()
    {
        
    }
    
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClicked);
        playerNameInputField.onValueChanged.RemoveListener(OnPlayerNameFieldChanged);
    }
}

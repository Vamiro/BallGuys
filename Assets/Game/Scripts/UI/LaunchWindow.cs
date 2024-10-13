using Photon.Pun;
using TMPro;
using UnityEngine;
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

    protected override void OnShow(params object[] args)
    {
        
    }
    
    private void OnPlayButtonClicked()
    {
        launcher.Connect();
    }

    private void OnPlayerNameFieldChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey,value);
    }

    private void InitializeNameField()
    {
        if (playerNameInputField == null || !PlayerPrefs.HasKey(playerNamePrefKey)) return;
        var defaultName = PlayerPrefs.GetString(playerNamePrefKey);
        playerNameInputField.text = defaultName;
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

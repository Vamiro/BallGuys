using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WebSocketSharp;

public class LaunchWindow : BaseWindow
{
    private const string PlayerNamePrefKey = "PlayerName";
    
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    
    public Button PlayButton => playButton;
    public UnityAction OnPlay;

    private void Awake()
    {
        playButton.onClick.AddListener(() => OnPlay?.Invoke());
        playerNameInputField.onValueChanged.AddListener(OnPlayerNameFieldChanged);
    }

    private void Start()
    {
        InitializeNameField();
    }

    protected override void OnShow(params object[] args)
    {
        
    }

    private void OnPlayerNameFieldChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(PlayerNamePrefKey,value);
    }

    private void InitializeNameField()
    {
        if (PlayerPrefs.HasKey(PlayerNamePrefKey))
        {
            playerNameInputField.text = PlayerPrefs.GetString(PlayerNamePrefKey);
        }
        else
        {
            playerNameInputField.text = "Player" + Random.Range(1000, 10000);
        }
    }

    protected override void OnHide()
    {
        
    }
    
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(() => OnPlay?.Invoke());
        playerNameInputField.onValueChanged.RemoveListener(OnPlayerNameFieldChanged);
    }
}

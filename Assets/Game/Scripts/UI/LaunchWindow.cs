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
    [SerializeField] private Button trainButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    
    public Button PlayButton => playButton;
    public UnityAction OnPlay;
    public Button TrainButton => trainButton;
    public UnityAction OnTrain;

    private void Awake()
    {
        playButton.onClick.AddListener(() => OnPlay?.Invoke());
        trainButton.onClick.AddListener(() => OnTrain?.Invoke());
        exitButton.onClick.AddListener(Application.Quit);
        playerNameInputField.onDeselect.AddListener(OnPlayerNameFieldDeselected);
    }

    private void Start()
    {
        InitializeNameField();
    }

    protected override void OnShow(params object[] args)
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
        playButton.onClick.RemoveListener(() => OnPlay?.Invoke());
        trainButton.onClick.RemoveListener(() => OnTrain?.Invoke());
        exitButton.onClick.RemoveListener(Application.Quit);
        playerNameInputField.onDeselect.RemoveListener(OnPlayerNameFieldDeselected);
    }
}

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    private LaunchWindow launchWindow;

    [SerializeField] [Tooltip("The UI Label to inform the user that the connection is in progress")]
    private ConnectWindow connectWindow;

    [SerializeField] private int maxPlayers = 4;
    [SerializeField] private int minPlayers = 2;

    [SerializeField] private int timeToWait = 10;
    private float _currentTime;
    private bool _readyToStart;

    /// <summary>
    ///     This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking
    ///     changes).
    /// </summary>
    private readonly string _gameVersion = "1";

    /// <summary>
    ///     Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    ///     we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    ///     Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    private bool _isConnecting;

    private bool _isTrainning;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        launchWindow.OnPlay += Connect;
        launchWindow.OnTrain += TrainConnect;
        launchWindow.Show();
        launchWindow.PlayButton.interactable = false;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected && !_isConnecting)
        {
            launchWindow.PlayButton.interactable = false;
            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }

        if (!_readyToStart) return;
        _currentTime -= Time.deltaTime;
        if (!(_currentTime <= 0) && PhotonNetwork.CurrentRoom.PlayerCount != maxPlayers) return;
        _readyToStart = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("MainLevel");
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected) return;
        launchWindow.Hide();
        connectWindow.Show();
        PhotonNetwork.JoinRandomRoom();
    }

    private void TrainConnect()
    {
        if (!PhotonNetwork.IsConnected) return;
        launchWindow.Hide();
        connectWindow.Show();

        var roomName = "TrainRoom" + PhotonNetwork.CountOfRooms;
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 1,
            IsOpen = false,
            IsVisible = false
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        _isTrainning = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered room");
        if (PhotonNetwork.CurrentRoom.PlayerCount < minPlayers || !PhotonNetwork.IsMasterClient) return;
        _currentTime = timeToWait;
        _readyToStart = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left room");
        if (PhotonNetwork.CurrentRoom.PlayerCount < minPlayers || !PhotonNetwork.IsMasterClient) return;
        _readyToStart = false;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player entered room");
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.CurrentRoom.PlayerCount < minPlayers && !_isTrainning) return;
        _currentTime = timeToWait;
        _readyToStart = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateAndJoinRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CreateAndJoinRoom();
    }

    private void CreateAndJoinRoom()
    {
        var roomName = "Room" + PhotonNetwork.CountOfRooms;

        var roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayers,
            IsOpen = true,
            IsVisible = true
        };

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        _isConnecting = false;
        launchWindow.PlayButton.interactable = true;
        launchWindow.TrainButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectWindow.Hide();
        launchWindow.Show();

        launchWindow.PlayButton.interactable = false;
        launchWindow.TrainButton.interactable = false;
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}",
            cause);
    }
}
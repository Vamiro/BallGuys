using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private LaunchWindow launchWindow;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private ConnectWindow connectWindow;

    [SerializeField]
    private int maxPlayers = 4;

    [SerializeField]
    private int minPlayers = 2;

    [SerializeField]
    private int timeToWait = 10;
    private float _currentTime;
    private bool _readyToStart;
    
    private bool _isFindingRoom;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string _gameVersion = "1";

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool _isConnecting;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    void Start()
    {
        launchWindow.OnPlay += Connect;
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
        if (!(_currentTime <= 0)) return;
        _readyToStart = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("MainLevel");
    }

    private void CreateRoom()
    {
        string roomName = "Room " + Random.Range(1000, 10000);
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = maxPlayers,
            IsOpen = true,
            IsVisible = true,
            EmptyRoomTtl = 0,
            PlayerTtl = 0,
        };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected) return;
        launchWindow.Hide();
        connectWindow.Show();
        Debug.Log(PhotonNetwork.CountOfRooms + " rooms available");
        PhotonNetwork.JoinRandomRoom();
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
        if (PhotonNetwork.CurrentRoom.PlayerCount < minPlayers || !PhotonNetwork.IsMasterClient) return;
        _currentTime = timeToWait;
        _readyToStart = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No room available, creating one...");
        CreateRoom();
    }

    public override void OnConnectedToMaster()
    {
        _isConnecting = false;
        launchWindow.PlayButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectWindow.Hide();
        launchWindow.Show();
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
}

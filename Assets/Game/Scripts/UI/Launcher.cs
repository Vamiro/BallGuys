using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    
    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 10;
    
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private LaunchWindow launchWindow;
    
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private ConnectWindow connectWindow;
    
    #endregion

    #region Private Fields

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

    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        //Connect();
        launchWindow.Show();
    }

    #endregion

    #region Private Methods
    
    void CreateRoom()
    {
        string roomName = "Room " + Random.Range(1000, 10000);
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayersPerRoom };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }
    
    #endregion

    #region Public Methods

    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        launchWindow.Hide();
        connectWindow.Show();
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }

    #endregion
    
    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        if (_isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            _isConnecting = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectWindow.Hide();
        launchWindow.Show();
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No random room available, creating a new room.");
        CreateRoom();
        
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            Debug.Log($"We load the 'Room for 2'");
            PhotonNetwork.LoadLevel("Room for 2");
        }
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }
    
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            Debug.Log($"We load the 'Room for 2'");
            PhotonNetwork.LoadLevel("Room for 2");
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
    }
    
    #endregion

}

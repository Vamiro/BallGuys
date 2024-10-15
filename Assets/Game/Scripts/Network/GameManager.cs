using Game.Scripts.UI;
using Game.Scripts.Utilities;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonPunCallbacks<GameManager>
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    
    [SerializeField] private WinWindow winWindow;
    private void Start()
    {
#if UNITY_5_4_OR_NEWER
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }
        else
        {
            if (PlayerController.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
        
        winWindow.OnExit += LeaveRoom;
    }

    public void Win()
    {
        if (!photonView.IsMine) return;
        ShowWinWindow(PhotonNetwork.NickName);
        photonView.RPC(nameof(ShowWinWindow), RpcTarget.Others, PhotonNetwork.NickName);
    }
    
    [PunRPC]
    private void ShowWinWindow(string winner)
    {
        winWindow.Show(winner);
    }

    public override void OnLeftRoom()
    {
        if (PlayerController.LocalPlayerInstance)
        {
            PhotonNetwork.Destroy(PlayerController.LocalPlayerInstance);
        }
        
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        if (PlayerController.LocalPlayerInstance)
        {
            PhotonNetwork.Destroy(PlayerController.LocalPlayerInstance);
        }
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " players in room");
        PhotonNetwork.LeaveRoom();
    }

#if !UNITY_5_4_OR_NEWER
    /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
#endif

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }
    
#if UNITY_5_4_OR_NEWER
    public override void OnDisable()
    {
        // Always call the base to remove callbacks
        base.OnDisable ();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
#endif
    
#if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
#endif
}

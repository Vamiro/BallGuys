using Game.Scripts.UI;
using Photon.Pun;
using UnityEngine;

public class FinishVolume : MonoBehaviourPunCallbacks
{
    [SerializeField] private WinWindow winWindow;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !photonView.IsMine) return;
        ShowWinWindow(PhotonNetwork.NickName);
        photonView.RPC(nameof(ShowWinWindow), RpcTarget.Others, PhotonNetwork.NickName);
    }
    
    [PunRPC]
    private void ShowWinWindow(string winner)
    {
        winWindow.Show(winner);
    }
}

using Game.Scripts.UI;
using Photon.Pun;
using UnityEngine;

public class FinishVolume : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !photonView.IsMine) return;
        GameManager.Instance.Win();
    }
}

using Game.Scripts.UI;
using Photon.Pun;
using UnityEngine;

public class FinishVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || other.gameObject != PlayerController.LocalPlayerInstance) return;
        GameManager.Instance.Finish();
    }
}

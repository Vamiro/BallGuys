using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.gameObject.GetComponent<PlayerController>().Respawn();
    }
}

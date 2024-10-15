using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.gameObject.GetComponent<PlayerController>().Respawn();
    }
}

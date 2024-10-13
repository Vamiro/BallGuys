using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPoint;
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.transform.position = spawnPoint;
    }
}

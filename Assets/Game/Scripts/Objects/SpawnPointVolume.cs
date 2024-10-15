using System;
using UnityEngine;

public class SpawnPointVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        other.gameObject.GetComponent<PlayerController>().SetSpawnPoint(transform.position);
    }
}

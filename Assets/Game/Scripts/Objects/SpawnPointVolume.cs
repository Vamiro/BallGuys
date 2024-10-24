using System;
using UnityEngine;

[Serializable]
public struct SpawnPoint
{
    public int Index;
    public Vector3 Position;
}

public class SpawnPointVolume : MonoBehaviour
{
    [SerializeField] private ParticleSystem spawnActivatedEffect;
    [SerializeField] private SpawnPoint spawnPoint;

    private void Start()
    {
        spawnPoint.Position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") || other.gameObject != PlayerController.LocalPlayerInstance) return;
        var player = other.GetComponent<PlayerController>();
        if (!player || player.spawnPoint.Index == spawnPoint.Index) return;
        spawnActivatedEffect.Play();
        other.gameObject.GetComponent<PlayerController>().SetSpawnPoint(spawnPoint);
    }
}
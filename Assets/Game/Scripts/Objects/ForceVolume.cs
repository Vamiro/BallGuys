using Photon.Pun;
using UnityEngine;

public enum ForceType
{
    Force,
    Impulse
}

public class ForceVolume : MonoBehaviourPunCallbacks
{
    [SerializeField] private new Collider collider;
    [SerializeField] private new ParticleSystem particleSystem;
    
    [Header("Force")] [SerializeField] private ForceType forceType;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float strength;

    [Header("Horizontal Random Direction")]
    [SerializeField] private bool isRandom;
    [SerializeField] private float randomTime;

    private float _randomTime;

    private void Start()
    {
        if (!collider) Debug.LogError("<Color=Red><a>Missing</a></Color> Collider Component on ForceVolume.", this);
        _randomTime = randomTime;
        SyncParticles();
    }

    private void Update()
    {
        if (!photonView.IsRoomView || !isRandom) return;
        _randomTime -= Time.deltaTime;

        if (_randomTime > 0) return;
        photonView.RPC(nameof(ChangeDirection), RpcTarget.All,
            new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized);
        _randomTime = randomTime;
    }

    [PunRPC]
    private void ChangeDirection(Vector3 newDirection)
    {
        direction = newDirection;
        SyncParticles();
    }

    private void SyncParticles()
    {
        if (!particleSystem || forceType != ForceType.Force) return;

        var shape = particleSystem.shape;
        shape.scale = collider.bounds.size;

        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.xMultiplier = direction.x * strength;
        velocityOverLifetime.zMultiplier = direction.z * strength;

        if (particleSystem.main.loop) return;
        var main = particleSystem.main;
        main.loop = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || forceType != ForceType.Impulse) return;

        other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * strength, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") || forceType != ForceType.Force) return;
        Debug.Log("Addin force");
        other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * strength, ForceMode.Force);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (collider.transform == null) return;

        Gizmos.DrawRay(collider.bounds.center, direction * strength);
        Gizmos.DrawIcon(collider.bounds.center, "science.png", true);
    }
}
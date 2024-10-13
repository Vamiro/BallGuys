using UnityEngine;

public enum ForceType
{
    Force,
    Impulse
}

public class ForceVolume : MonoBehaviour
{
    [SerializeField] private new Collider collider;
    [SerializeField] private ForceType forceType;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float strength;

    private void Start()
    {
        if (!collider) Debug.LogError("<Color=Red><a>Missing</a></Color> Collider Component on ForceVolume.", this);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || forceType != ForceType.Impulse) return;
        
        other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * strength, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") || forceType != ForceType.Force) return;
        
        other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * strength, ForceMode.Force);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (collider.transform == null) return;
        
        Gizmos.DrawRay(collider.bounds.center, direction);
        Gizmos.DrawIcon(collider.bounds.center, "science.png", true);
    }
}



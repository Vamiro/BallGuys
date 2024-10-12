using Game.Scripts;
using UnityEngine;

[RequireComponent(typeof(TimingAction), typeof(Collider), typeof(Renderer))]
public class ImpulseTrap : MonoBehaviour
{
    public Rigidbody playerRb;
    public Vector3 impulseDirection;
    public float impulseForce;
    
    private TimingAction _timingAction;
    private Renderer _renderer;

    void Start()
    {
        _timingAction = GetComponent<TimingAction>();
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = Color.green;
        _timingAction.OnSleeping += () =>
        {
            _renderer.material.color = Color.green;
            if (playerRb) _timingAction.Activate();
        };
        _timingAction.OnWaiting += () => _renderer.material.color = Color.yellow;
        _timingAction.Action += ThrowPlayer;
        _timingAction.OnCoolingDown += () => _renderer.material.color = Color.blue;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || PlayerController.LocalPlayerInstance != other.gameObject) return;
        
        playerRb = other.gameObject.GetComponent<Rigidbody>();
        _timingAction.Activate();
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerController.LocalPlayerInstance == other.gameObject)
        {
            playerRb = null;
        }
    }
    
    private void ThrowPlayer()
    {
        _renderer.material.color = Color.red;
        if (playerRb)
        {
            playerRb.AddForce(impulseDirection.normalized * impulseForce, ForceMode.Impulse);
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, impulseDirection.normalized * impulseForce);
    }
}

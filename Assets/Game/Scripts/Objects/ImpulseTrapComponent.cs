using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(TimingActionComponent))]
public class ImpulseTrapComponent : MonoBehaviour
{
    [SerializeField] private TimingActionComponent timingActionComponent;
    
    [Header("Impulse")]
    [SerializeField] private Vector3 impulseDirection;
    [SerializeField] private float impulseForce;
    
    [Header("Renderer")]
    [SerializeField] private Renderer mRenderer;
    [SerializeField] private Material sleepingMaterial;
    [SerializeField] private Material waitingMaterial;
    [SerializeField] private Material actionMaterial;
    [SerializeField] private Material coolingDownMaterial;

    private Rigidbody _playerRb;

    void Start()
    {
        mRenderer.material = sleepingMaterial;

        timingActionComponent.OnSleeping -= TimingActionOnSleeping;
        timingActionComponent.OnSleeping += TimingActionOnSleeping;
        
        timingActionComponent.OnWaiting -= TimingActionOnWaiting;
        timingActionComponent.OnWaiting += TimingActionOnWaiting;
        
        timingActionComponent.OnAction -= ThrowPlayer;
        timingActionComponent.OnAction += ThrowPlayer;
        
        timingActionComponent.OnCoolingDown -= TimingActionOnCoolingDown;
        timingActionComponent.OnCoolingDown += TimingActionOnCoolingDown;
    }

    private void OnDestroy()
    {
        timingActionComponent.OnSleeping -= TimingActionOnSleeping;
        timingActionComponent.OnWaiting -= TimingActionOnWaiting;
        timingActionComponent.OnAction -= ThrowPlayer;
        timingActionComponent.OnCoolingDown -= TimingActionOnCoolingDown;
    }

    private void TimingActionOnSleeping()
    {
        mRenderer.material = sleepingMaterial;

        if (!_playerRb) return;
        timingActionComponent.Activate();
    }

    private void TimingActionOnWaiting()
    {
        mRenderer.material = waitingMaterial;
    }

    private void ThrowPlayer()
    {
        mRenderer.material = actionMaterial;
        if (!_playerRb) return;
        _playerRb.AddForce(impulseDirection.normalized * impulseForce, ForceMode.Impulse);
    }

    private void TimingActionOnCoolingDown()
    {
        mRenderer.material = coolingDownMaterial;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || PlayerController.LocalPlayerInstance != other.gameObject) return;
        _playerRb = other.gameObject.GetComponent<Rigidbody>();
        timingActionComponent.Activate();
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || PlayerController.LocalPlayerInstance != other.gameObject) return;
        _playerRb = null;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, impulseDirection.normalized * impulseForce);
    }
}

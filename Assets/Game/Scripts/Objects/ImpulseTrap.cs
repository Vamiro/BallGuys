using UnityEngine;

[RequireComponent(typeof(TimingAction))]
public class ImpulseTrap : MonoBehaviour
{
    [SerializeField] private TimingAction timingAction;
    
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

        timingAction.OnSleeping -= TimingActionOnSleeping;
        timingAction.OnSleeping += TimingActionOnSleeping;
        
        timingAction.OnWaiting -= TimingActionOnWaiting;
        timingAction.OnWaiting += TimingActionOnWaiting;
        
        timingAction.OnAction -= ThrowPlayer;
        timingAction.OnAction += ThrowPlayer;
        
        timingAction.OnCoolingDown -= TimingActionOnCoolingDown;
        timingAction.OnCoolingDown += TimingActionOnCoolingDown;
    }

    private void OnDestroy()
    {
        timingAction.OnSleeping -= TimingActionOnSleeping;
        timingAction.OnWaiting -= TimingActionOnWaiting;
        timingAction.OnAction -= ThrowPlayer;
        timingAction.OnCoolingDown -= TimingActionOnCoolingDown;
    }

    private void TimingActionOnSleeping()
    {
        mRenderer.material = sleepingMaterial;

        if (!_playerRb) return;
        timingAction.Activate();
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
        timingAction.Activate();
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

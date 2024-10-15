using System;
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
    private Material[] _materials;
    void Start()
    {
        
        _materials = mRenderer.materials;
        _materials[1] = sleepingMaterial;
        mRenderer.materials = _materials;

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
        _materials = mRenderer.materials;
        _materials[1] = sleepingMaterial;
        mRenderer.materials = _materials;

        if (!_playerRb) return;
        timingActionComponent.Activate();
    }

    private void TimingActionOnWaiting()
    {
        _materials = mRenderer.materials;
        _materials[1] = waitingMaterial;
        mRenderer.materials = _materials;
    }

    private void ThrowPlayer()
    {
        _materials = mRenderer.materials;
        _materials[1] = actionMaterial;
        mRenderer.materials = _materials;
        if (!_playerRb) return;
        _playerRb.AddForce(impulseDirection.normalized * impulseForce, ForceMode.Impulse);
    }

    private void TimingActionOnCoolingDown()
    {
        _materials = mRenderer.materials;
        _materials[1] = coolingDownMaterial;
        mRenderer.materials = _materials;
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

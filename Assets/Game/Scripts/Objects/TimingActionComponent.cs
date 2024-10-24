using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public enum ActionStatus
{
    Sleeping,
    Waiting,
    CoolingDown
}

public class TimingActionComponent : MonoBehaviourPunCallbacks
{
    [Tooltip("Time to wait before executing the action"), SerializeField]
    private float timeToWait;
    private float _currentTimeToWait;

    [Tooltip("Cooldown time after executing the action"), SerializeField]
    private float cooldown;
    private float _currentCooldown;
    
    private ActionStatus _status = ActionStatus.Sleeping;
    
    public UnityAction OnSleeping;
    public UnityAction OnWaiting;
    public UnityAction OnAction;
    public UnityAction OnCoolingDown;

    private void Update()
    {
        switch (_status)
        {
            case ActionStatus.Sleeping:
                break;

            case ActionStatus.Waiting:
                _currentTimeToWait -= Time.deltaTime;
                if (_currentTimeToWait > 0) break;

                _status = cooldown > 0 ? ActionStatus.CoolingDown : ActionStatus.Sleeping;
                _currentCooldown = cooldown;

                try
                {
                    OnAction?.Invoke();
                    OnCoolingDown?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                break;

            case ActionStatus.CoolingDown:
                _currentCooldown -= Time.deltaTime;
                if (_currentCooldown > 0) break;
                _status = ActionStatus.Sleeping;
                
                try
                {
                    OnSleeping?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [PunRPC]
    public void Activate()
    {
        if (_status != ActionStatus.Sleeping) return;
        _status = ActionStatus.Waiting;
        OnWaiting?.Invoke();
        _currentTimeToWait = timeToWait;
        photonView.RPC("Activate", RpcTarget.Others);
    }
}
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public enum ActionStatus
{
    Sleeping,
    Waiting,
    CoolingDown
}

public class TimingAction : MonoBehaviourPunCallbacks
{
    [Tooltip("Time to wait before executing the action")]
    public float timeToWait;
    private float _currentTimeToWait;

    [Tooltip("Cooldown time after executing the action")]
    public float cooldown;
    private float _currentCooldown;

    public UnityAction OnSleeping;
    public UnityAction OnWaiting;
    public UnityAction Action;
    public UnityAction OnCoolingDown;
    
    private ActionStatus _status = ActionStatus.Sleeping;
    
    private void Update()
    {
        switch (_status)
        {
            case ActionStatus.Sleeping:
                break;
            case ActionStatus.Waiting:
                _currentTimeToWait -= Time.deltaTime;
                if (_currentTimeToWait <= 0)
                {
                    _status = cooldown > 0 ? ActionStatus.CoolingDown : ActionStatus.Sleeping;
                    _currentCooldown = cooldown;
                    Action?.Invoke();
                    OnCoolingDown?.Invoke();
                }
                break;
            case ActionStatus.CoolingDown:
                _currentCooldown -= Time.deltaTime;
                if (_currentCooldown <= 0)
                {
                    _status = ActionStatus.Sleeping;
                    OnSleeping?.Invoke();
                }
                break;
        }
    }
    
    [PunRPC]
    public void Activate()
    {
        if (_status == ActionStatus.Sleeping)
        {
            _status = ActionStatus.Waiting;
            OnWaiting?.Invoke();
            _currentTimeToWait = timeToWait;
            photonView.RPC("Activate", RpcTarget.Others);
        }
    }
}
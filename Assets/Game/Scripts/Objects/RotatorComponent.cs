using System;
using UnityEngine;

public enum RotationAxis
{
    X,
    Y,
    Z
}

public class RotatorComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RotationAxis rotationAxis;
    [SerializeField] private float speed;

    private void Start()
    {
        if (!rb) Debug.LogError("<Color=Red><a>Missing</a></Color> Rigidbody Component on Rotator.", this);
    }

    void Update()
    {
        rb.angularVelocity = rotationAxis switch
        {
            RotationAxis.X => transform.right * speed,
            RotationAxis.Y => transform.up * speed,
            RotationAxis.Z => transform.forward * speed,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
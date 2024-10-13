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
        switch (rotationAxis)
        {
            case RotationAxis.X:
                rb.angularVelocity = transform.right * speed;
                break;
            case RotationAxis.Y:
                rb.angularVelocity = transform.up * speed;
                break;
            case RotationAxis.Z:
                rb.angularVelocity = transform.forward * speed;
                break;
            default:
                throw new Exception("Invalid Rotation Axis");
        }
    }
}
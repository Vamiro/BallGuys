using System;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public enum RotationAxis
{
    X,
    Y,
    Z
}

public class Rotator : MonoBehaviour
{
    public Rigidbody rb;
    
    public RotationAxis rotationAxis;
    public float speed;
    
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
        }
    }
}
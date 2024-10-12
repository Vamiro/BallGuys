using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum ForceType
{
    Force,
    Impulse
}

[RequireComponent(typeof(Collider))]
public class ForceVolume : MonoBehaviour
{
    public new Collider collider;
    public ForceType forceType;
    public Vector3 direction;
    public float strength;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Player") && forceType == ForceType.Impulse)
        {
            Debug.Log("Impulse");
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * strength, ForceMode.Impulse);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && forceType == ForceType.Force)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * strength, ForceMode.Force);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (collider.transform != null)
        {
            Gizmos.DrawRay(collider.bounds.center, direction);
            Gizmos.DrawIcon(collider.bounds.center, "science.png", true);
        }
    }
}



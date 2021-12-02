﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    //Скрипт - притяжение к центру планеты

    [SerializeField] private float strenghtGravity;
    private HashSet<Rigidbody> affectedBodies = new HashSet<Rigidbody>();
    private Rigidbody componentRigidbody;

    private void Start()
    {
        componentRigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            affectedBodies.Add(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            affectedBodies.Remove(other.attachedRigidbody);
        }
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody body in affectedBodies)
        {
            Vector3 forceDirection = (transform.position - body.position).normalized;
            float distanceSqr = (transform.position - body.position).sqrMagnitude;
            float strength = strenghtGravity * componentRigidbody.mass * body.mass / distanceSqr;

            body.AddForce(forceDirection * strength);
        }
    }
}

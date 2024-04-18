using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public GameObject drone;
    public Rigidbody rb;

    public float lift = 1;

    void Start()
    {
        
    }

    void Update()
    {
        rb.AddForceAtPosition(transform.up * lift, drone.transform.position);
    }
}

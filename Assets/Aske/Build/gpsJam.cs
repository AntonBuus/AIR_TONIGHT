using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gpsJam : MonoBehaviour
{
    [SerializeField] GameObject waypoints;
    [SerializeField] AutoPilot autoPilot;


    [SerializeField] private GameObject drone;

    [SerializeField] private float waypointDistanceThresh = 50;

    // Start is called before the first frame update
    void Start()
    {
        //waypoints = GetComponent<Waypoints>();
        //autoPilot = GetComponent<AutoPilot>();


    }

    // Update is called once per frame
    void Update()
    {
        InitializeEmergency();
    }
    public void InitializeEmergency()
    {
        Vector3 waypointDistance = waypoints.transform.GetChild(2).transform.position;

        float distanceToWaypoint = Vector3.Distance(waypointDistance, drone.transform.position);

        print(distanceToWaypoint);

        if(distanceToWaypoint < waypointDistanceThresh) 
        { 
            autoPilot.enabled = false;

        }

    }

    // Out of line of sight, fly back to checkpoint.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "waypointX")
        {
            // don't follow waypoint

        }
        if (other.tag == "waypointY")
        {
            // follow waypoint
        }

    }

}

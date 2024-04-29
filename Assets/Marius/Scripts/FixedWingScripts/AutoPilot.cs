using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    
    //[SerializeField] private GameObject waypoint;

    private Waypoints waypoints;

    private Transform currentWaypoint;

    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float distanceThreshHold = 100;

    private void Start()
    {
        waypoints = FindObjectOfType<Waypoints>();

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        //transform.position = currentWaypoint.position;

        //Set next waypoint target
        //currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        Vector3 tempWaypoint = (currentWaypoint.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(tempWaypoint);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position,currentWaypoint.position) < distanceThreshHold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

            print("Checkpoint reached");
        }

        
    }
}

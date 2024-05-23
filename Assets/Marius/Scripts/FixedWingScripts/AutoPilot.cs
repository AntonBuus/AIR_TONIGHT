using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    #region Variables
    //[SerializeField] private GameObject waypoint;

    private AircraftPhysics aircraftPhysics;

    private Waypoints waypoints;

    private Transform currentWaypoint;

    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float distanceThreshHold = 100;
    private float thrustPercent;

    #endregion

    #region Main Methods
    private void Start()
    {
        waypoints = FindObjectOfType<Waypoints>();

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        aircraftPhysics = GetComponent<AircraftPhysics>();

        //transform.position = currentWaypoint.position;

        //Set next waypoint target
        //currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //thrustPercent = FWInputs.Throttle;
            thrustPercent = thrustPercent > 0 ? 0 : 1f;
        }
    }
    #endregion

    #region Costum Methods
     public void ToggleThrottle()
    {
        thrustPercent = thrustPercent > 0 ? 0 : 1f;
        aircraftPhysics.SetThrustPercent(thrustPercent);
        print(thrustPercent);
    }

    #endregion
}

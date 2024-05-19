using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTLWaypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float wayPointSize = 1f;

    private void OnDrawGizmos() //Only runs in scene view
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, wayPointSize);
        }

        Gizmos.color = Color.blue;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }

    public Transform GetNextWaypoint(Transform activeWaypoint)
    {
        if (activeWaypoint == null) //If no waypoint is active, set the next waypoint to the first waypoint in the hierachy.
        {
            return transform.GetChild(0).transform;
        }

        //If the active waypoint's placement in the hierachy is less than the total amount of waypoints, set the next waypoint as the active waypoint.
        if (activeWaypoint.GetSiblingIndex() < transform.childCount - 1) //-1 because the GetSiblingIndex counts from 0 while the childCount starts from 1. 
        {
            return transform.GetChild(activeWaypoint.GetSiblingIndex() + 1).transform; //Returns the transform of the next waypoint in the hieracy
        }
        else
        {
            return transform.GetChild(0); //If not more waypoints in the hierachy, start over from the first waypoint
        }
    }
}

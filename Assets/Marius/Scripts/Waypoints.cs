using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float wayPointSize = 1f;

    private void OnDrawGizmos() //Only runs in scene view
    {
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(t.position, wayPointSize);
        }

        Gizmos.color = Color.red;
        for(int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if(currentWaypoint == null)
        {
            return transform.GetChild(0).transform;
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1) //-1 because we start at 0 and not 1. So it goes, 0, 1, 2...
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1).transform;
        }
        else
        {
            return transform.GetChild(0);
        }
    }
}

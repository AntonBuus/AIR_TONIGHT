using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletConnLoss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collision course, killswitch.
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

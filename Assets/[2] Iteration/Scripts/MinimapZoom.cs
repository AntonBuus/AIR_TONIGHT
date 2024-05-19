using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MinimapZoom : MonoBehaviour
{
    public Camera _cameraToZoom;

    private void OnTriggerExit(Collider collider)
    {
       if(collider.tag == "Drone_Graphic")
        {
            _cameraToZoom.orthographicSize = 900;
        }
            
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Drone_Graphic")
        {
            _cameraToZoom.orthographicSize = 500;
        }
    }

}

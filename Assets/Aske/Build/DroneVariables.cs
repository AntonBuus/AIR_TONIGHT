using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

public class DroneVariables : MonoBehaviour
{
    public TMP_Text speedText;
    public TMP_Text heightText;
    public TMP_Text clearanceText;

    private Rigidbody rb;
    private float droneHeight;
    private float hoverError;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v3Vel = rb.velocity;
        float vel=v3Vel.magnitude;
        float velKMH = vel * 3.6f;
        speedText.text = "Speed:\n" + velKMH.ToString("F2")+ " km/h";
        droneHeight = transform.position.y;
        heightText.text = "Height:\n" + droneHeight.ToString("F2") + " m";
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        float droneClearance=terrainHeight-droneHeight;
        float posDroneClearance = Mathf.Abs(droneClearance);
        clearanceText.text = "Clearance:\n" + posDroneClearance.ToString("F2") + " m";

        //Debug.Log("Drone height: " + droneHeight + " " + "Terrain height: " + terrainHeight+"Raycast: "+hoverError);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);


        if (Physics.Raycast(downRay, out hit))
        {
            // The "error" in height is the difference between the desired height
            // and the height measured by the raycast distance.
            hoverError = droneHeight - hit.distance;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

public class DroneVariables : MonoBehaviour
{
    Rigidbody rb;
    public TMP_Text speedText;
    public TMP_Text heightText;
    public TMP_Text clearanceText;
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
        speedText.text = "Speed: "+vel*3.6+" km/h";
        float droneHeight = transform.position.y;
        heightText.text = "Height: " + droneHeight+ " m";
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        float droneClearance=terrainHeight-droneHeight;
        clearanceText.text = "Clearance: " + Mathf.Abs(droneClearance) + " m";
    }
}
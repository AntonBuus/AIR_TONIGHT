using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DroneVariables : MonoBehaviour
{
    [SerializeField] private TMP_Text clearanceText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text altitudeText;
    [SerializeField] private TMP_Text latitudeText;
    [SerializeField] private TMP_Text longitudeText;
    [SerializeField] private TMP_Text throttleText;


    private Rigidbody rb;
    private FixedWing_Controller _fixedWing_Controller;

    public float _droneVelocity; //Made public by Marius to be used in NewHUD.cs
    [SerializeField] private float _droneHeight;
    [SerializeField] private float _droneClearance;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _fixedWing_Controller = FindObjectOfType<FixedWing_Controller>();
    }

    void Update()
    {
        clearanceText.text = _droneClearance.ToString("F2") + " m";

        Vector3 v3Vel = rb.velocity;
        _droneVelocity = v3Vel.magnitude;
        //float velKMH = _droneVelocity * 3.6f;
        //speedText.text = "Speed:\n" + velKMH.ToString("F2")+ " km/h";
        speedText.text = _droneVelocity.ToString("F2")+ " m/s";

        float _droneHeight = transform.position.y;
        float _droneLatitude = transform.position.x;
        float _droneLongitude = transform.position.z;
        altitudeText.text = _droneHeight.ToString("F2") + " m";
        latitudeText.text = _droneLatitude.ToString("F2");
        longitudeText.text = _droneLongitude.ToString("F2");
        throttleText.text = (int)(_fixedWing_Controller.thrustPercent * 100) + " %";
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);

        // Cast a ray straight downwards.
        if (Physics.Raycast(downRay, out hit))
        {
            _droneClearance = hit.distance;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Placed on drone

public class CrashLandingDetection : MonoBehaviour
{
    private Menu_Manager menuManager;

    [Tooltip("Assign prefab with explosion here")]
    [SerializeField] private GameObject crashExplosion;

    public bool droneCrashed = false;
    public bool droneLanded;

    private bool toggleSwitch = false;

    private EmergencyController _emergencyController;
    private DroneVariables _droneVariables;
    private int _emergencyBegun;

    private Rigidbody rigidbody;

    private void Start()
    {
        menuManager = FindObjectOfType<Menu_Manager>();
        _emergencyController = FindObjectOfType<EmergencyController>();
        _droneVariables = FindAnyObjectByType<DroneVariables>();
        rigidbody = GetComponent<Rigidbody>();
    }

    //Checks for crash
    private void OnTriggerEnter(Collider other) //Check if the trigger colliders collides with anything.
    {
        Instantiate(crashExplosion, transform.position, Quaternion.identity);

        droneCrashed = true;
        droneLanded = false;

        menuManager.MissionEndScreen();

        this.gameObject.SetActive(false);
    }

    //Checks for landing 
    private void OnCollisionStay(Collision collision) //Checks if the colliders on the drone's wheels are touching the ground
    {
        if (!droneCrashed && _emergencyController.emergencyBegun > 0 && rigidbody.velocity.magnitude < 2) //if the drone is not crashed, emergency begun is more than 0, and the drones speed is less than 2.
        {
            if(toggleSwitch == false)
            {
                toggleSwitch = true;
                menuManager.MissionEndScreen();
            }
        }

    }

    private void OnCollisionExit(Collision collision) //Checks if the colliders on the drone's wheels aren't touching the ground
    {
        droneLanded = false;
        toggleSwitch = false;
    }
}

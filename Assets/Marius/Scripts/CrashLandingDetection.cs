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

    private EmergencyController _emergencyController;
    private DroneVariables _droneVariables;
    private int _emergencyBegun;

    private void Start()
    {
        menuManager = FindObjectOfType<Menu_Manager>();
        _emergencyController = FindObjectOfType<EmergencyController>();
        _droneVariables = FindAnyObjectByType<DroneVariables>();
    }
    private void OnTriggerEnter(Collider other) //Check if the trigger colliders collides with anything.
    {
        try
        {
            menuManager.MissionEndScreen();
        }
        catch
        {
            print("Menu_Manager object not found in scene");
        }
        
        droneCrashed = true;
        droneLanded = false;
        Instantiate(crashExplosion, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionStay(Collision collision) //Checks if the colliders on the drone's wheels are touching the ground
    {
        if (!droneCrashed && _emergencyController.emergencyBegun > 0 && _droneVariables._droneVelocity < 1)
        {
            droneLanded = true;
            menuManager.MissionEndScreen();
        }
    }

    private void OnCollisionExit(Collision collision) //Checks if the colliders on the drone's wheels aren't touching the ground
    {
        droneLanded = false;
    }
}

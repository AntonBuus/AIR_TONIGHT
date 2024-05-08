using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmergencyController : MonoBehaviour
{
    public GameObject drone;
    private EmergencyManager _emergencyManager;
    private FixedWing_Controller _fixedWing_Controller;
    private Menu_Manager _menu_Manager;

    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform waypoint2;
    [SerializeField] private Transform waypoint3;
    [SerializeField] private float _threshold = 50;

    bool fwcEnabled = false;
    bool autopilotToggled = false;
    bool throttleToggled = false;

    private int _activeEmergency;
    public int emergencyBegun = 0;

    void Start()
    {
        _emergencyManager = FindObjectOfType<EmergencyManager>();

        _fixedWing_Controller = FindObjectOfType<FixedWing_Controller>();

        _menu_Manager = FindObjectOfType<Menu_Manager>();
    }

    private void Update()
    {
        switch (_emergencyManager.currentEmergency)
        {
            case 1:
                GpsJam();
                break;
            case 2:
                TabletConnLoss();
                break;
            case 3:
                BrokenOnTakeoff();
                break;
        }
    }

    // disable autopilot to imitate no gps signal.
    private void GpsJam()
    {
        if (Vector3.Distance(drone.transform.position,waypoint1.position) < _threshold)
        {

            emergencyBegun = 1;
            if (fwcEnabled == false)
            {
                fwcEnabled = true;
                _fixedWing_Controller.autoPilot = false;
            }
        }/*
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            _menu_Manager.MissionEndScreen();
        }*/
    }
    
    // disable autopilot and manualControl to imitate GPS and controller connection loss.
    private void TabletConnLoss()
    {
        //Should prevent ToggleThrottle button from working
        if (Vector3.Distance(drone.transform.position, waypoint2.position) < _threshold)
        {
            emergencyBegun = 2;
            if (autopilotToggled == false)
            {
                autopilotToggled = true;
                // No manual or autopilot controls.
                //_fixedWing_Controller.controlFailure = true;
            }
        }/*
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            _menu_Manager.MissionEndScreen();
        }*/
    }

    // disable throttle to imitate engine or propeller issue.
    private void BrokenOnTakeoff()
    {
        //broken sound
        //Should prevent ToggleThrottle button from working
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            emergencyBegun = 3;
            if (throttleToggled == false)
            {
                throttleToggled = true;
                _fixedWing_Controller.ToggleThrottle();
            }
        }/*
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            _menu_Manager.MissionEndScreen();
        }*/
    }
}
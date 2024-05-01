using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmergencyController : MonoBehaviour
{
    private GameObject drone;
    private EmergencyManager _emergencyManager;
    private FixedWing_Controller _fixedWing_Controller;

    [SerializeField] private gpsJam gpsJamScript;
    [SerializeField] private TabletConnLoss tabletConnLossScript;
    [SerializeField] private BrokenOnTakeoff brokenOnTakeoffScript;

    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform waypoint2;
    [SerializeField] private Transform waypoint3;

    [SerializeField] private float _thrustPercent;
    [SerializeField] private float _threshold = 50;

    bool fwcEnabled = false;
    bool autopilotToggled = false;
    bool throttleToggled = false;

    private int _activeEmergency;

    void Start()
    {
        _emergencyManager = FindObjectOfType<EmergencyManager>();
        _activeEmergency = _emergencyManager.currentEmergency;

        _fixedWing_Controller = FindObjectOfType<FixedWing_Controller>();
        _thrustPercent = _fixedWing_Controller.thrustPercent*100;
    }

    private void Update()
    {
        switch (_activeEmergency)
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
            if (fwcEnabled == false)
            {
                fwcEnabled = true;
                // No manual or autopilot controls.
                _fixedWing_Controller.controlFailure = true;
            }
        }
    }
    
    // disable autopilot and manualControl to imitate GPS and controller connection loss.
    private void TabletConnLoss()
    {
        //Should prevent ToggleThrottle button from working
        if (Vector3.Distance(drone.transform.position, waypoint2.position) < _threshold)
        {
            if (autopilotToggled == false)
            {
                autopilotToggled = true;
                _fixedWing_Controller.autoPilot = false;
            }
        }
    }

    // disable throttle to imitate engine or propeller issue.
    private void BrokenOnTakeoff()
    {
        //broken sound
        //Should prevent ToggleThrottle button from working
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            if (throttleToggled == false)
            {
                throttleToggled = true;
                _fixedWing_Controller.ToggleThrottle();
            }
        }
    }
}
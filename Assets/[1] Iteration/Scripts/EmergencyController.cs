using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmergencyController : MonoBehaviour
{
    public GameObject drone;
    private EmergencyManager _emergencyManager;
    private FixedWing_Controller _fixedWing_Controller;
    [SerializeField] private gpsJam gpsJamScript;
    [SerializeField] private TabletConnLoss tabletConnLossScript;
    [SerializeField] private BrokenOnTakeoff brokenOnTakeoffScript;

    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform waypoint2;
    [SerializeField] private Transform waypoint3;

    private float threshold = 50;

    bool fwcEnabled = false;
    bool autopilotToggled = false;
    bool throttleToggled = false;

    private int _activeEmergency;
    // Start is called before the first frame update
    void Start()
    {
        _emergencyManager = FindObjectOfType<EmergencyManager>();
        _activeEmergency = _emergencyManager.currentEmergency;

        _fixedWing_Controller = FindObjectOfType<FixedWing_Controller>();
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

    private void GpsJam()
    {
        if (Vector3.Distance(drone.transform.position,waypoint1.position) < threshold)
        {
            if (fwcEnabled == false)
            {
                fwcEnabled = true;
                //should be disabled via bools in fixedwing_controller.cs instead
                _fixedWing_Controller.enabled = false;
            }
        }
    }

    private void TabletConnLoss()
    {
        //Should prevent ToggleThrottle button from working
        if (Vector3.Distance(drone.transform.position, waypoint2.position) < threshold)
        {
            if (autopilotToggled == false)
            {
                autopilotToggled = true;
                _fixedWing_Controller.autoPilot = false;
            }
        }
    }

    private void BrokenOnTakeoff()
    {
        //broken sound
        //Should prevent ToggleThrottle button from working
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < threshold)
        {
            if (throttleToggled == false)
            {
                throttleToggled = true;
                _fixedWing_Controller.ToggleThrottle();
            }
        }
    }
}